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
        private readonly IOrderActivityService _orderActivityService;
        private readonly IMenuDetailService _menuDetailService;
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;
        public OrderService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
           IProfileService profileService,
           ISessionDetailService sessionDetailService,
           IOrderDetailService orderDetailService,
           IOrderActivityService orderActivityService,
           IMenuDetailService menuDetailService,
           ITransactionService transactionService,
           IWalletService walletService) : base(unitOfWork, mapper, appSettings)
        {
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _orderDetailService = orderDetailService;
            _orderActivityService = orderActivityService;
            _menuDetailService = menuDetailService;
            _transactionService = transactionService;
            _walletService = walletService;
        }

        public async Task<ICollection<GetOrderResponse>> GetAllAsync(string? userRole)
        {

            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = (o) => o.Include(o => o.Profile!).Include(o => o.SessionDetail!);

            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await _repository.GetListAsync<GetOrderResponse>(include: include);
            }

            return await _repository.GetListAsync<GetOrderResponse>(BaseEntityStatus.Active, include: include);

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
                page = await _repository.GetPageAsync<GetOrderResponse>(
                    paginationRequest: request, orderBy: orderBy);
            }
            else
            {
                page = await _repository.GetPageAsync<GetOrderResponse>(BaseEntityStatus.Active,
                    paginationRequest: request, orderBy: orderBy);
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

        public async Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status)
        {
            List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.Status == status
            };

            var orders = await _repository.GetListAsync(filters: filters,
                include: queryable => queryable.Include(o => o.Profile!).Include(o => o.SessionDetail!));

            return _mapper.Map<ICollection<GetOrderResponse>>(orders);
        }

        public async Task CreateOrderAsync(User user, CreateOrderRequest request)
        {
            var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, user.Id);
            var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);

            var orderActivityNumber = await _repository.CountAsync() + 1;
            var transactionNumber = await _repository.CountAsync() + 1;
            var orderNumber = await _repository.CountAsync() + 1;
            
            var orderId = Guid.NewGuid();
            var orderEntity = _mapper.Map<Order>(request);
            orderEntity.Id = orderId;
            orderEntity.PaymentDate = DateTime.Now;
            orderEntity.Status = OrderStatus.Cooking;
            orderEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderCodeConstrant.OrderPrefix, orderNumber);

            if (!(orderEntity.PaymentDate >= sessionDetail.Session!.OrderStartTime && orderEntity.PaymentDate < sessionDetail.Session!.OrderEndTime))
            {
                throw new ClosedSessionException();
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidSchoolException();
            }

            var orderDetailEntityList = new List<OrderDetail>();

            foreach (var orderDetailModel in request.OrderDetails!)
            {
                var orderDetailEntity = _mapper.Map<OrderDetail>(orderDetailModel);
                var menuDetail = await _menuDetailService.GetByIdAsync(orderDetailModel.MenuDetailId);
                orderDetailEntity.OrderId = orderId;
                orderDetailEntity.FoodId = menuDetail.FoodId;
                orderDetailEntity.Price = menuDetail.Price;
                orderDetailEntity.Status = OrderDetailStatus.Active;
                orderDetailEntityList.Add(orderDetailEntity);
            }
            orderEntity.OrderDetails?.Clear();

            var totalPriceOfOrderDetail = orderDetailEntityList.Sum(od => od.Quantity * od.Price);
            orderEntity.TotalPrice = totalPriceOfOrderDetail;

            var wallet = profile.Wallets!.FirstOrDefault(w => w.Type == WalletType.Money.ToString())!;
            if (wallet.Balance - totalPriceOfOrderDetail < 0)
            {
                throw new InvalidWalletBalanceException(MessageConstants.WalletMessageConstrant.NotEnoughMoney);
            }

            orderEntity.OrderActivities = new List<OrderActivity>
            {

            new OrderActivity
                {
                    Id = Guid.NewGuid(),
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                    Name = MessageConstants.OrderActivityMessageConstrant.OrderCookingActivityName,
                    Time = DateTime.Now,
                    Status = OrderActivityStatus.Active
                }
            };

            orderEntity.Transactions = new List<Transaction>
            {
                new Transaction
                {
                    OrderId = orderId,
                    ExchangeGiftId = null,
                    WalletId = wallet.Id,
                    Value = orderEntity.TotalPrice,
                    Time = DateTime.Now,
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, transactionNumber),
                    Status = TransactionStatus.Active
                }
            };

            wallet.Balance -= totalPriceOfOrderDetail;
            await _repository.InsertAsync(orderEntity);
            await _orderDetailService.CreateOrderDetailListAsync(orderDetailEntityList);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCompleteStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Completed;
            orderEntity.DeliveryDate = DateTime.Now;
            orderEntity.RewardPoints = CalculateRewardPoints(orderEntity.TotalPrice);

            orderEntity.OrderActivities = new List<OrderActivity>
            {

            new OrderActivity
                {
                    Id = Guid.NewGuid(),
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                    Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                    Time = DateTime.Now,
                    Status = OrderActivityStatus.Active
                }
            };

            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        //public async Task UpdateOrderCookingStatusAsync(Guid orderId)
        //{
        //    var orderEntity = await GetByIdAsync(orderId);
        //    orderEntity.Status = OrderStatus.Cooking;
        //    await _repository.UpdateAsync(orderEntity);
        //    await _unitOfWork.CommitAsync();
        //}

        public async Task UpdateOrderDeliveryStatusAsync(Guid foodId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(foodId);
            orderEntity.Status = OrderStatus.Delivering;

            orderEntity.OrderActivities = new List<OrderActivity>
            {

            new OrderActivity
                {
                    Id = Guid.NewGuid(),
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                    Name = MessageConstants.OrderActivityMessageConstrant.OrderDeliveringActivityName,
                    Time = DateTime.Now,
                    Status = OrderActivityStatus.Active
                }
            };
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCancelStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = (int)OrderStatus.Cancelled;

            orderEntity.OrderActivities = new List<OrderActivity>
            {

            new OrderActivity
                {
                    Id = Guid.NewGuid(),
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                    Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                    Time = DateTime.Now,
                    Status = OrderActivityStatus.Active
                }
            };

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
