using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.OrderActivity.Response;
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
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using System.Threading.Tasks;
using Utilities.Enums;
using DataTransferObjects.Models.OrderActivity.Request;
using Utilities.Utils;
using DataTransferObjects.Models.Notification.Request;

namespace Services.Implements
{
    public class OrderActivityService : BaseService<OrderActivity>, IOrderActivityService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly INotificationService _notificationService;
        public OrderActivityService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService, INotificationService notificationService) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _notificationService = notificationService;
        }

        public async Task<OrderActivity> GetByIdAsync(Guid id)
        {
            List<Expression<Func<OrderActivity, bool>>> filters = new()
            {
                (orderActivity) => orderActivity.Id == id
            };
            var orderActivity = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.OrderActivityMessageConstrant.OrderActivityNotFound(id));
            return orderActivity;
        }

        public async Task<GetOrderActivityResponse> GetOrderActivityResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetOrderActivityResponse>(await GetByIdAsync(id));
        }
        public async Task CreateOrderActivityAsync(Order order, OrderActivity orderActivity, User user)
        {
            orderActivity.OrderId = order.Id;
            
            if (user == null || order.Profile!.User!.Id != user.Id)
            {
                await _notificationService.SendNotificationAsync(new CreateNotificationRequest
                {
                    Body = orderActivity.Name,
                    Title = MessageConstants.NotificationMessageConstrant.OrderNotificationTitle(order.Code),
                    NotificationDetails = new List<CreateNotificationRequest.NotificationDetailOfCreateNotificationRequest>
                    {
                        new ()
                        {
                            UserId = order.Profile!.UserId,
                        }
                    }
                });
            }
            await _repository.InsertAsync(orderActivity, user);
            await _unitOfWork.CommitAsync();
        }
        public async Task CreateOrderActivityAsync(CreateOrderActivityRequest request, User user)
        {
            var orderActivity = _mapper.Map<OrderActivity>(request);
            var orderActivityId = Guid.NewGuid();
            orderActivity.Id = orderActivityId;

            if (request.OrderId != null && request.OrderId != Guid.Empty)
            {
                orderActivity.OrderId = request.OrderId;
            }
            else if (request.ExchangeGiftId != null && request.ExchangeGiftId != Guid.Empty)
            {
                orderActivity.ExchangeGiftId = request.ExchangeGiftId;
            }
            if (request.Image != null)
            {
                orderActivity.ImagePath = await _cloudStorageService.UploadFileAsync(orderActivityId, _appSettings.Firebase.FolderNames.OrderActivity, request.Image);
            }
            orderActivity.Time = TimeUtil.GetCurrentVietNamTime();
            orderActivity.Status = OrderActivityStatus.Active;
            var orderActivityNumber = await _repository.CountAsync() + 1;
            orderActivity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber);
            await _repository.InsertAsync(orderActivity, user);
            await _unitOfWork.CommitAsync();
        }


        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user)
        {
            var roleName = user.Role!.EnglishName;
            List<Expression<Func<OrderActivity, bool>>> filters = new();
            if (RoleName.CUSTOMER.ToString().Equals(roleName))
            {
                filters.Add(oa => oa.Order!.Profile!.UserId == user.Id);
            }
            filters.Add(oa => oa.OrderId == orderId);
            var result = await _repository.GetListAsync<GetOrderActivityResponse>(filters: filters);
            return result;
            //await _repository.GetListAsync()
        }

        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user)
        {
            var roleName = user.Role!.EnglishName;
            List<Expression<Func<OrderActivity, bool>>> filters = new();
            if (RoleName.CUSTOMER.ToString().Equals(roleName))
            {
                filters.Add(oa => oa.Order!.Profile!.UserId == user.Id);
            }
            filters.Add(oa => oa.ExchangeGiftId == exchangeGiftId);
            var result = await _repository.GetListAsync<GetOrderActivityResponse>(filters: filters);
            return result;
        }

        public async Task CreateOrderActivityAsync(ExchangeGift exchangeGift, OrderActivity orderActivity, User user)
        {
            orderActivity.OrderId = exchangeGift.Id;
            if (user == null || exchangeGift.Profile!.User!.Id != user.Id)
            {
                await _notificationService.SendNotificationAsync(new CreateNotificationRequest
                {
                    Body = orderActivity.Name,
                    Title = MessageConstants.NotificationMessageConstrant.OrderNotificationTitle(exchangeGift.Code),
                    NotificationDetails = new List<CreateNotificationRequest.NotificationDetailOfCreateNotificationRequest>
                {
                    new ()
                    {
                        UserId = exchangeGift.Profile!.UserId,
                    }
                }
                });
            }
            await _repository.InsertAsync(orderActivity, user);
            await _unitOfWork.CommitAsync();
        }
    }
}
