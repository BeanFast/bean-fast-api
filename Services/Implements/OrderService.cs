﻿using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Exceptions;
using Utilities.Constants;
using Utilities.Utils;

namespace Services.Implements
{
    public class OrderService : BaseService<Order>, IOrderService
    {
        private readonly IProfileService _profileService;
        private readonly ISessionDetailService _sessionDetailService;
        private readonly IOrderDetailService _orderDetailService;
        public OrderService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IProfileService profileService, ISessionDetailService sessionDetailService, IOrderDetailService orderDetailService) : base(unitOfWork, mapper, appSettings)
        {
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _orderDetailService = orderDetailService;
        }

        public async Task<ICollection<GetOrderResponse>> GetAllAsync(string? userRole)
        {
            Expression<Func<Order, GetOrderResponse>> selector = (o => _mapper.Map<GetOrderResponse>(o));

            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = (o) => o.Include(o => o.Profile!).Include(o => o.SessionDetail!);

            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await _repository.GetListAsync(include: include, selector: selector);
            }

            return await _repository.GetListAsync(BaseEntityStatus.Active, include: include, selector: selector);

        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.Id == id
            };
            var order = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(o => o.Profile!).Include(o => o.SessionDetail!))
                ?? throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.OrderNotFound(id));
            return order;
        }

        public async Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetOrderResponse>(await GetByIdAsync(id));
        }

        public async Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request)
        {
            Expression<Func<Order, GetOrderResponse>> selector = (o => _mapper.Map<GetOrderResponse>(o));
            Func<IQueryable<Order>, IOrderedQueryable<Order>> orderBy = o => o.OrderBy(o => o.DeliveryDate);
            IPaginable<GetOrderResponse>? page = null;
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                page = await _repository.GetPageAsync(
                    paginationRequest: request, selector: selector, orderBy: orderBy);
            }
            else
            {
                page = await _repository.GetPageAsync(BaseEntityStatus.Active,
                    paginationRequest: request, selector: selector, orderBy: orderBy);
            }
            return page;
        }

        public async Task<ICollection<GetOrderResponse>> GetOrdersByProfileIdAsync(Guid profileId, Guid userId)
        {
            var profile = await _profileService.GetByIdAsync(profileId);

            if (profile.UserId != userId)
            {
                throw new ProfileNotMatchException();
            }

            List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.ProfileId == profileId
            };

            var orders = await _repository.GetListAsync(filters: filters,
                include: queryable => queryable.Include(o => o.Profile!).Include(o => o.SessionDetail!));

            return _mapper.Map<ICollection<GetOrderResponse>>(orders);
        }

        public async Task CreateOrderAsync(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();
            var orderEntity = _mapper.Map<Order>(request);
            await _profileService.GetByIdAsync(request.ProfileId);
            await _sessionDetailService.GetByIdAsync(request.SessionDetailId);
            orderEntity.Id = orderId;
            orderEntity.PaymentDate = DateTime.Now;
            orderEntity.Status = OrderStatus.Pending;
            orderEntity.Code = EntityCodeUtil.GenerateUnnamedEntityCode(EntityCodeConstrant.OrderCodeConstrant.OrderPrefix, orderId);
            var orderDetailEntityList = new List<OrderDetail>();

            if (request.OrderDetails is not null && request.OrderDetails.Count > 0)
            {
                foreach (var orderDetail in request.OrderDetails)
                {
                    var orderDetailEntity = new OrderDetail
                    {
                        OrderId = orderId,
                        FoodId = orderDetail.FoodId,
                        Quantity = orderDetail.Quantity,
                        Note = orderDetail.Note
                    };
                    orderDetailEntityList.Add(orderDetailEntity);
                }
            }
            orderEntity.OrderDetails?.Clear();
            await _repository.InsertAsync(orderEntity);
            await _orderDetailService.CreateOrderDetailListAsync(orderDetailEntityList);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCompleteStatusAsync(Guid orderId)
        {
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Completed;
            orderEntity.DeliveryDate = DateTime.Now;
            orderEntity.RewardPoints = CalculateRewardPoints(orderEntity.TotalPrice);
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCookingStatusAsync(Guid orderId)
        {
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Cooking;
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderDeliveryStatusAsync(Guid foodId)
        {
            var orderEntity = await GetByIdAsync(foodId);
            orderEntity.Status = OrderStatus.Delivering;
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCancelStatusAsync(Guid orderId)
        {
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = (int)OrderStatus.Cancelled;
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task FeedbackOrderAsync(Guid orderId, FeedbackOrderRequest request)
        {
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Feedback = request.Feedback;
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Guid guid)
        {
            var order = await GetByIdAsync(guid);
            await _repository.DeleteAsync(order);
            await _unitOfWork.CommitAsync();
        }

        public static int CalculateRewardPoints(double totalOrderPrice)
        {
            double points = totalOrderPrice / 1000;
            return (int)Math.Round(points, MidpointRounding.AwayFromZero);
        }
    }
}
