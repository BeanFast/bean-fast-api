using AutoMapper;
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
using System.Linq.Expressions;
using Utilities.Enums;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Exceptions;
using Utilities.Constants;
using Utilities.Utils;
using DataTransferObjects.Models.OrderActivity.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.IdentityModel.Tokens;

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
        private readonly ILoyaltyCardService _loyaltyCardService;
        public OrderService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
           IProfileService profileService,
           ISessionDetailService sessionDetailService,
           IOrderDetailService orderDetailService,
           IOrderActivityService orderActivityService,
           IMenuDetailService menuDetailService,
           ITransactionService transactionService,
           IWalletService walletService,
           ILoyaltyCardService loyaltyCardService) : base(unitOfWork, mapper, appSettings)
        {
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _orderDetailService = orderDetailService;
            _orderActivityService = orderActivityService;
            _menuDetailService = menuDetailService;
            _transactionService = transactionService;
            _walletService = walletService;
            _loyaltyCardService = loyaltyCardService;
        }
        private List<Expression<Func<Order, bool>>> GetFiltersFromOrderRequest(OrderFilterRequest request)
        {
            List<Expression<Func<Order, bool>>> filters = new();
            if (request.Status != null)
            {
                filters.Add(o => o.Status == request.Status);
            }
            return filters;
        }
        public async Task<ICollection<GetOrderResponse>> GetAllAsync(OrderFilterRequest request, User user)
        {
            var filters = GetFiltersFromOrderRequest(request);
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include = (o) => o.Include(o => o.Profile!).Include(o => o.SessionDetail!);

            if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName))
            {
                //filters.Add()
            }else if(RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName))
            {
                filters.Add(o => o.Profile!.UserId == user.Id);
            }
            else if (RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName))
            {
                filters.Add(o => o.SessionDetail!.DelivererId == user.Id);   
            }

            return await _repository.GetListAsync<GetOrderResponse>(filters: filters, include: include);

        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.Id == id
            };
            var order = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(o => o.Profile!)
                .ThenInclude(p => p.Wallets)
                .Include(o => o.SessionDetail!)
                .Include(o => o.OrderActivities!))
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
                throw new InvalidRequestException(MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser);
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

        public async Task<ICollection<Order>> GetOrdersDeliveringByProfileIdAndDelivererId(Guid profileId, Guid delivererId)
        {
            List<Expression<Func<Order, bool>>> filters = new()
            {
                (order) => order.ProfileId == profileId
                && order.SessionDetail!.DelivererId == delivererId
                && order.Status == OrderStatus.Delivering
            };
            Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include =
                (order) => order.Include(o => o.SessionDetail!).ThenInclude(sd => sd.Session!);

            //var orders = await _repository.GetListAsync(filters: filters,
            //    status: OrderStatus.Delivering, include: queryable => queryable
            //    .Include(o => o.SessionDetail!)
            //    .ThenInclude(sd => sd.Session!))
            //    ?? throw new EntityNotFoundException("Không có order nào đang giao hết");
            return await _repository.GetListAsync(include: include, filters: filters);
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
            orderEntity.PaymentDate = TimeUtil.GetCurrentVietNamTime();
            orderEntity.Status = OrderStatus.Cooking;
            orderEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderCodeConstrant.OrderPrefix, orderNumber);

            if (!(orderEntity.PaymentDate >= sessionDetail.Session!.OrderStartTime && orderEntity.PaymentDate < sessionDetail.Session!.OrderEndTime))
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed);
            }
            if (sessionDetail.Location!.SchoolId != profile.SchoolId)
            {
                throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.InvalidSchoolLocation);
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

            var wallet = profile.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))!;
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
                    Time = TimeUtil.GetCurrentVietNamTime(),
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
                    Time = TimeUtil.GetCurrentVietNamTime(),
                    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, transactionNumber),
                    Status = TransactionStatus.Active
                }
            };

            wallet.Balance -= totalPriceOfOrderDetail;
            await _repository.InsertAsync(orderEntity);
            await _orderDetailService.CreateOrderDetailListAsync(orderDetailEntityList);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderStatusByQRCodeAsync(string qrCode, Guid delivererId)
        {
            bool isUpdated = false;
            var loyaltyCard = await _loyaltyCardService.GetLoyaltyCardByQRCode(qrCode);
            var profileId = loyaltyCard.ProfileId;
            var orderList = await GetOrdersDeliveringByProfileIdAndDelivererId(profileId, delivererId);
            if(orderList.IsNullOrEmpty())
            {
                throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NoDeliveryOrders);
            }   
            var timeScanning = TimeUtil.GetCurrentVietNamTime();
            foreach (var order in orderList)
            {
                if (timeScanning >= order.SessionDetail!.Session!.DeliveryStartTime && timeScanning < order.SessionDetail!.Session!.DeliveryEndTime)
                {
                    await UpdateOrderCompleteStatusAsync(order.Id);
                    isUpdated = true;
                }
                if(!isUpdated)
                {
                    throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionDeliveryClosed);
                }
            }
        }

        public async Task UpdateOrderCompleteStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Completed;
            orderEntity.DeliveryDate = TimeUtil.GetCurrentVietNamTime();
            orderEntity.RewardPoints = CalculateRewardPoints(orderEntity.TotalPrice);
            
            orderEntity.OrderActivities!.Add(new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            });

            var wallet = orderEntity.Profile!.Wallets!.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type))!;
            wallet.Balance += orderEntity.RewardPoints;

            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateOrderDeliveryStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Delivering;

            orderEntity.OrderActivities!.Add(new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            });

            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderCancelStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _repository.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = (int)OrderStatus.Cancelled;

            orderEntity.OrderActivities!.Add(new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            });

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

        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user)
        {
            var orderActivities = await _orderActivityService.GetOrderActivitiesByOrderIdAsync(orderId, user);
            return orderActivities;
        }

        public async Task CreateOrderActivityAsync(CreateOrderActivityRequest request)
        {
            request.ExchangeGiftId = null;
            if(request.OrderId == null || request.OrderId == Guid.Empty)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderIdRequired);
            }
            await _orderActivityService.CreateOrderActivityAsync(request);
        }
    }
}
