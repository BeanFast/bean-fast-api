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
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
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
        private readonly IFoodService _foodService;
        private readonly IUserService _userService;
        private readonly IOrderRepository _repository;

        public OrderService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings,
           IProfileService profileService,
           ISessionDetailService sessionDetailService,
           IOrderDetailService orderDetailService,
           IOrderActivityService orderActivityService,
           IMenuDetailService menuDetailService,
           ITransactionService transactionService,
           IWalletService walletService,
           IFoodService foodService, IUserService userService, IOrderRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _profileService = profileService;
            _sessionDetailService = sessionDetailService;
            _orderDetailService = orderDetailService;
            _orderActivityService = orderActivityService;
            _menuDetailService = menuDetailService;
            _transactionService = transactionService;
            _walletService = walletService;
            _foodService = foodService;
            _userService = userService;
            _repository = repository;
        }
        

        public async Task<ICollection<GetOrderResponse>> GetAllAsync(OrderFilterRequest request, User user)
        {
            return await _repository.GetAllAsync(request, user);
        }
        public async Task<IPaginable<GetOrderResponse>> GetPageAsync(PaginationRequest paginationRequest, OrderFilterRequest request, User user)
        {
            return await _repository.GetPageAsync(paginationRequest, request, user);
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<GetOrderByIdResponse> GetOderResponseByIdAsync(Guid id)
        {
            return await _repository.GetOderResponseByIdAsync(id);
        }

        public async Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request)
        {
            return await _repository.GetPageAsync(userRole, request);
        }

        //public async Task<ICollection<GetOrderResponse>> GetOrdersByProfileIdAsync(Guid profileId, Guid userId)
        //{
        //    var profile = await _profileService.GetByIdAsync(profileId);

        //    if (profile.UserId != userId)
        //    {
        //        throw new InvalidRequestException(MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser);
        //    }

        //    List<Expression<Func<Order, bool>>> filters = new()
        //    {
        //        (order) => order.ProfileId == profileId
        //    };

        //    var orders = await _repository.GetListAsync(filters: filters,
        //        include: queryable => queryable.Include(o => o.Profile!).Include(o => o.SessionDetail!));

        //    return _mapper.Map<ICollection<GetOrderResponse>>(orders);
        //}

        public async Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status)
        {
            return await _repository.GetOrdersByStatusAsync(status);
        }

        public  Task<ICollection<GetOrderResponse>> GetOrdersDeliveringByProfileIdAndDelivererId(Guid profileId, Guid delivererId)
        {
            //List<Expression<Func<Order, bool>>> filters = new()
            //{
            //    (order) => order.ProfileId! == profileId
            //    && order.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == delivererId)
            //    && order.Status == OrderStatus.Delivering
            //};
            //Func<IQueryable<Order>, IIncludableQueryable<Order, object>> include =
            //    (order) => order.Include(o => o.OrderDetails!).Include(o => o.SessionDetail!).ThenInclude(sd => sd.Session!);

            ////var orders = await _repository.GetListAsync(filters: filters,
            ////    status: OrderStatus.Delivering, include: queryable => queryable
            ////    .Include(o => o.SessionDetail!)
            ////    .ThenInclude(sd => sd.Session!))
            ////    ?? throw new EntityNotFoundException("Không có order nào đang giao hết");
            //var orders = await _repository.GetListAsync(include: include, filters: filters);
            //return _mapper.Map<ICollection<GetOrderResponse>>(orders);
            return null;
        }
        public async Task<List<GetOrderResponse>> GetValidOrderResponsesByQRCodeAsync(string qrCode, Guid delivererId)
        {
            var customer = await _userService.GetCustomerByQrCodeAsync(qrCode);
            var orders = await GetDeliveringOrdersByDelivererIdAndCustomerIdAsync(delivererId, customer.Id);
            if (orders.IsNullOrEmpty())
            {
                throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NoDeliveryOrders);
            }
            var timeScanning = TimeUtil.GetCurrentVietNamTime();
            var validOrders = new List<GetOrderResponse>();
            foreach (var order in orders)
            {
                if (timeScanning >= order.SessionDetail!.Session!.DeliveryStartTime && timeScanning < order.SessionDetail!.Session!.DeliveryEndTime)
                {

                    validOrders.Add(_mapper.Map<GetOrderResponse>(order));
                }
            }
            if (validOrders.Count == 0)
            {
                throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NotFoundOrders);
            }
            return validOrders;
        }
        public async Task<ICollection<Order>> GetDeliveringOrdersByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId)
        {
            return await _repository.GetDeliveringOrdersByDelivererIdAndCustomerIdAsync(delivererId, customerId);
        }

        public async Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user)
        {
            var orderActivities = await _orderActivityService.GetOrderActivitiesByOrderIdAsync(orderId, user);
            return orderActivities;
        }
        public async Task<ICollection<GetOrdersByLastMonthsResponse>> GetOrdersByLastMonthsAsync(GetOrdersByLastMonthsRequest request)
        {
            //var today = TimeUtil.GetCurrentVietNamTime();

            DateTime pastMonthStart = request.StartDate;
            var allMonths = Enumerable.Range(0, request.EndDate.Month - request.StartDate.Month).Select(i => pastMonthStart.AddMonths(i)).ToList();

            var orders = await _repository.GetOrdersAsync(
                request.StartDate,
                request.EndDate,
                request.Status
            );
            var data = orders.GroupBy(order => order.PaymentDate.Month)
                .OrderBy(group => group.Key)
                .Select(
                    group => new GetOrdersByLastMonthsResponse
                    {
                        MonthInt = group.Key,
                        Month = TimeUtil.GetMonthName(group.Key),
                        Count = group.Count(),
                        Revenue = int.Parse(group.Sum(order => order.TotalPrice).ToString())
                    }
                ).ToList();
            //var missingDates = allMonths.Except(data.Select(d => d.DateTime));
            foreach (var month in allMonths)
            {
                if (!data.Any(d => d.Month == TimeUtil.GetMonthName(month.Month)))
                {
                    data.Add(new GetOrdersByLastMonthsResponse
                    {
                        MonthInt = month.Month,
                        Month = TimeUtil.GetMonthName(month.Month),
                        Count = 0,
                        Revenue = 0
                    });
                }
            }
            return data.OrderBy(d => d.MonthInt).ToList();
        }
        public async Task<ICollection<GetOrdersByLastDaysResponse>> GetOrdersByLastDatesAsync(int numberOfDate)
        {
            DateTime yesterday = DateTime.Today.Subtract(TimeSpan.FromDays(1));
            DateTime pastWeekStart = yesterday.Subtract(TimeSpan.FromDays(numberOfDate));
            var orders = await _repository.GetOrdersAsync(pastWeekStart, yesterday, OrderStatus.Completed);
            List<GetOrdersByLastDaysResponse> result = new List<GetOrdersByLastDaysResponse>();
            var data = orders.GroupBy(order => order.PaymentDate.Date)
                .OrderBy(group => group.Key)
                .Select(group => new GetOrdersByLastDaysResponse
                {
                    DateTime = group.Key,
                    Day = group.Key.Day + "/" + group.Key.Month,
                    Count = group.Count(),
                    Revenue = int.Parse(group.Sum(order => order.TotalPrice).ToString())
                })
                .ToList();

            var allDates = Enumerable.Range(0, numberOfDate).Select(i => pastWeekStart.AddDays(i)).ToList();
            var missingDates = allDates.Except(data.Select(d => d.DateTime));

            foreach (var date in missingDates)
            {
                data.Add(new GetOrdersByLastDaysResponse
                {
                    DateTime = date,
                    Day = date.Day + "/" + date.Month,
                    Count = 0,
                    Revenue = 0
                });
            }

            return data.OrderBy(d => d.DateTime).ToList();
        }
        public async Task<ICollection<GetTopSchoolBestSellerResponse>> GetTopSchoolBestSellers(int topCount)
        {
            var orders = await _repository.GetCompletedOrderIncludeSchoolAsync();
            var totalSoldCount = orders.Sum(order => order.OrderDetails!.Sum(od => od.Quantity));
            var data = orders.GroupBy(order => order.SessionDetail!.Location!.School!.Name)
                .Select(group => new GetTopSchoolBestSellerResponse
                {

                    SchoolName = group.Key,
                    Percentage = group.Sum(order => order.OrderDetails!.Sum(od => od.Quantity)) / (double)totalSoldCount * 100,
                    Revenue = int.Parse(group.Sum(order => order.TotalPrice).ToString()),
                    Count = group.Count()
                })
                .OrderByDescending(o => o.Percentage)
                .ToList();
            var topSchool = data.Take(topCount).ToList();
            if (data.Count() > topCount)
            {
                var otherData = data.Skip(topCount);
                topSchool.Add(new GetTopSchoolBestSellerResponse
                {
                    SchoolName = "Others",
                    Percentage = otherData.Sum(x => x.Percentage),
                    Revenue = otherData.Sum(x => x.Revenue),
                    Count = otherData.Sum(x => x.Count)
                });
            }
            var roundedData = topSchool.Select(s => new GetTopSchoolBestSellerResponse
            {
                SchoolName = s.SchoolName,
                Percentage = Math.Round(s.Percentage, 1),
                Revenue = s.Revenue,
                Count = s.Count
            }).ToList();

            // Calculate the total after rounding
            double totalAfterRounding = roundedData.Sum(c => c.Percentage);

            if (totalAfterRounding != 100)
            {
                roundedData.Last().Percentage += 100 - totalAfterRounding;
            }
            return roundedData;
        }

        public async Task<ICollection<GetTopBestSellerKitchenResponse>> GetTopBestSellerKitchens(int topCount, bool orderDesc)
        {
            var orders = await _repository.GetCompletedOrderIncludeKitchenAsync();
            var totalSoldCount = orders.Sum(order => order.OrderDetails!.Sum(od => od.Quantity));
            var data = orders.SelectMany(order => order.OrderDetails!)
                .GroupBy(od => od.Food!.MenuDetails!.First().Menu!.Kitchen!.Name) // Group by kitchen name
                .Select(group => new GetTopBestSellerKitchenResponse
                {
                    Name = group.Key,
                    TotalOrder = group.Count(),
                    TotalItem = group.Sum(od => od.Quantity),
                    Percentage = group.Sum(od => od.Quantity) / (double)totalSoldCount * 100
                })
                .ToList();
            if (orderDesc)
            {
                data = data.OrderByDescending(o => o.Percentage).ToList();
            }
            else
            {
                data = data.OrderBy(o => o.Percentage).ToList();
            }
            var topKitchen = data.Take(topCount).ToList();
            if (data.Count() > topCount)
            {
                var otherData = data.Skip(topCount);
                topKitchen.Add(new GetTopBestSellerKitchenResponse
                {
                    Name = "Others",
                    TotalOrder = otherData.Sum(x => x.TotalOrder),
                    TotalItem = otherData.Sum(x => x.TotalItem),
                    Percentage = otherData.Sum(x => x.Percentage)
                });
            }
            var roundedData = topKitchen.Select(s => new GetTopBestSellerKitchenResponse
            {
                Name = s.Name,
                TotalOrder = s.TotalOrder,
                TotalItem = s.TotalItem,
                Percentage = Math.Round(s.Percentage, 1)
            }).ToList();

            // Calculate the total after rounding
            double totalAfterRounding = roundedData.Sum(c => c.Percentage);

            if (totalAfterRounding != 100)
            {
                roundedData.Last().Percentage += 100 - totalAfterRounding;
            }
            if (!orderDesc)
            {
                roundedData.RemoveAll(item => item.Name == "Others");
            }
            return roundedData;
        }


        public async Task CreateOrderAsync(User user, CreateOrderRequest request)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                var profile = await _profileService.GetProfileByIdAndCurrentCustomerIdAsync(request.ProfileId, user.Id);
                var sessionDetail = await _sessionDetailService.GetByIdAsync(request.SessionDetailId);

                var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
                var transactionNumber = await _transactionService.CountAsync() + 1;
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
                if (TimeUtil.GetCurrentVietNamTime() >= sessionDetail.Session!.OrderEndTime)
                {
                    throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderClosed);
                }
                else if (TimeUtil.GetCurrentVietNamTime() < sessionDetail.Session!.OrderStartTime)
                {
                    throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.SessionOrderNotStarted);
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
                    var food = await _foodService.GetByIdAsync(menuDetail.FoodId);
                    orderDetailEntity.OrderId = orderId;
                    orderDetailEntity.FoodId = menuDetail.FoodId;
                    orderDetailEntity.Price = menuDetail.Price;
                    orderDetailEntity.Status = OrderDetailStatus.Active;
                    orderDetailEntityList.Add(orderDetailEntity);

                    // food?
                }
                orderEntity.OrderDetails?.Clear();

                var totalPriceOfOrderDetail = orderDetailEntityList.Sum(od => od.Quantity * od.Price);
                orderEntity.TotalPrice = totalPriceOfOrderDetail;

                var wallet = user.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type))!;
                if (wallet.Balance - totalPriceOfOrderDetail < 0)
                {
                    throw new InvalidWalletBalanceException(MessageConstants.WalletMessageConstrant.NotEnoughMoney);
                }
                wallet.Balance -= totalPriceOfOrderDetail;
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
                        Value = - orderEntity.TotalPrice,
                        Time = TimeUtil.GetCurrentVietNamTime(),
                        Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, transactionNumber),
                        Status = TransactionStatus.Active
                    }
                };
                await _walletService.UpdateAsync(wallet);
                await _repository.InsertAsync(orderEntity, user);
                await _orderDetailService.CreateOrderDetailListAsync(orderDetailEntityList);
                await _unitOfWork.CommitAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await Console.Out.WriteLineAsync(ex.Message.ToString());
                //if (ex is BeanFastApplicationException)
                throw;
            }
        }


        //public async Task UpdateOrderStatusByQRCodeAsync(string qrCode, User deliverer)
        //{
        //    bool isUpdated = false;
        //    var loyaltyCard = await _loyaltyCardService.GetLoyaltyCardByQRCode(qrCode);
        //    var profileId = loyaltyCard.ProfileId;
        //    var orderList = await GetOrdersDeliveringByProfileIdAndDelivererId(profileId, deliverer.Id);
        //    if (orderList.IsNullOrEmpty())
        //    {
        //        throw new EntityNotFoundException(MessageConstants.OrderMessageConstrant.NoDeliveryOrders);
        //    }
        //    var timeScanning = TimeUtil.GetCurrentVietNamTime();
        //    foreach (var order in orderList)
        //    {
        //        if (timeScanning >= order.SessionDetail!.Session!.DeliveryStartTime && timeScanning < order.SessionDetail!.Session!.DeliveryEndTime)
        //        {
        //            await UpdateOrderCompleteStatusAsync(order.Id, deliverer);
        //            isUpdated = true;
        //        }
        //        if (!isUpdated)
        //        {
        //            throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionDeliveryClosed);
        //        }
        //    }
        //}
        public async Task UpdateOrderCompleteStatusAsync(Guid orderId, User deliverer)
        {
            var startTime = DateTime.Now;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            var transactionNumber = await _transactionService.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            if (orderEntity.Status != OrderStatus.Delivering)
            {
                throw new InvalidRequestException("Bạn chỉ có thể hoàn thành các đơn hàng đang ở trạng thái đang giao");
            }
            if (TimeUtil.GetCurrentVietNamTime() > orderEntity.SessionDetail!.Session!.DeliveryEndTime)
            {
                throw new InvalidRequestException("Bạn không thể hoàn thành đơn hàng này vì đã hết thời gian giao hàng");
            }
            orderEntity.Status = OrderStatus.Completed;
            orderEntity.DeliveryDate = TimeUtil.GetCurrentVietNamTime();
            orderEntity.RewardPoints = CalculateRewardPoints(orderEntity.TotalPrice);
            var wallet = orderEntity.Profile!.Wallets!.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type))!;
            wallet.Balance += orderEntity.RewardPoints;

            var newOrderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            };
            var newTransaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.TransactionPrefix, transactionNumber),
                WalletId = wallet.Id,
                Value = orderEntity.RewardPoints,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = TransactionStatus.Active,
                OrderId = orderEntity.Id,
            };

            await _orderActivityService.CreateOrderActivityAsync(orderEntity, newOrderActivity, deliverer);
            await _transactionService.CreateTransactionAsync(newTransaction);
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
            var endTime = DateTime.Now;
            var delay = endTime - startTime;
            Console.WriteLine($"Delay: {delay.TotalMilliseconds} milliseconds");
        }
        //public async Task UpdateOrderDeliveringFromCompleteStatusAsync(Guid orderId)
        //{
        //    //var orderActivityNumber = await _repository.CountAsync() + 1;
        //    //var orderEntity = await GetByIdAsync(orderId);
        //    //orderEntity.Status = OrderStatus.Delivering;
        //    //orderEntity.DeliveryDate = null;
        //    //orderEntity.RewardPoints = 0;
        //    //var wallet = orderEntity.Profile!.Wallets!.FirstOrDefault(w => WalletType.Points.ToString().Equals(w.Type))!;
        //    //wallet.Balance += orderEntity.RewardPoints;
        //    //var newOrderActivity = new OrderActivity
        //    //{   
        //    //    Id = Guid.NewGuid(),
        //    //    Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
        //    //    Name = MessageConstants.OrderActivityMessageConstrant.OrderCompletedActivityName,
        //    //    Time = TimeUtil.GetCurrentVietNamTime(),
        //    //    Status = OrderActivityStatus.Active
        //    //};
        //    //await _orderActivityService.CreateOrderActivityAsync(orderEntity, newOrderActivity);
        //    //await _repository.UpdateAsync(orderEntity);
        //    //await _unitOfWork.CommitAsync();
        //}



        public async Task UpdateOrderDeliveryStatusAsync(Guid orderId)
        {
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            var orderEntity = await GetByIdAsync(orderId);
            orderEntity.Status = OrderStatus.Delivering;


            //await RollbackMoneyAsync(orderEntity);
            await _orderActivityService.CreateOrderActivityAsync(orderEntity, new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderDeliveringActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            }, null!);
            await _repository.UpdateAsync(orderEntity);
            await _unitOfWork.CommitAsync();
        }


        public async Task CancelOrderForManagerAsync(Order orderEntity, CancelOrderRequest request, User manager)
        {
            orderEntity.Status = OrderStatus.Cancelled;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCanceledActivityName + request.Reason,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active,
                OrderId = orderEntity.Id
            };
            await RollbackMoneyAsync(orderEntity);
            await _orderActivityService.CreateOrderActivityAsync(orderEntity, orderActivity, manager);
            orderEntity.Profile = null;
            await _repository.UpdateAsync(orderEntity, manager);
            await _unitOfWork.CommitAsync();
        }
        public async Task RollbackMoneyAsync(Order orderEntity)
        {

            var moneyWallet = await _walletService.GetMoneyWalletByUserId(orderEntity.Profile!.UserId);
            moneyWallet!.Balance += orderEntity.TotalPrice;
            var rollbackMoneyTransaction = new Transaction
            {
                OrderId = orderEntity.Id,
                Id = Guid.NewGuid(),
                Value = orderEntity.TotalPrice,
                WalletId = moneyWallet!.Id,
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.TransactionCodeConstrant.OrderTransactionPrefix, await _transactionService.CountAsync() + 1)
            };
            await _walletService.UpdateAsync(moneyWallet);
            await _transactionService.CreateTransactionAsync(rollbackMoneyTransaction);
        }
        public async Task CancelOrderForCustomerAsync(Order orderEntity, CancelOrderRequest request, User customer)
        {
            var validTime = TimeUtil.GetCurrentVietNamTime();
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;
            if (orderEntity.Profile!.User!.Id != customer.Id)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderNotBelongToThisUser);
            }
            if (validTime >= orderEntity.SessionDetail!.Session!.OrderStartTime &&
                validTime < orderEntity.SessionDetail!.Session!.OrderEndTime)
            {
                // hoan tien
                //var moneyWallet = orderEntity.Profile!.User!.Wallets!.FirstOrDefault(w => WalletType.Money.ToString().Equals(w.Type));
                //if (moneyWallet != null)
                //{
                await RollbackMoneyAsync(orderEntity);
                //}
            }

            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.ExchangeGiftCanceledByCustomerActivityName(request.Reason),
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active
            };
            await _orderActivityService.CreateOrderActivityAsync(orderEntity, orderActivity, customer);
            orderEntity.Status = OrderStatus.CancelledByCustomer;
            orderEntity.Profile = null;
            await _repository.UpdateAsync(orderEntity, customer);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateOrderStatusAfterDeliveryTimeEndedAsync()
        {
            bool isUpdated = false;
            var realTime = TimeUtil.GetCurrentVietNamTime();
            var listOrderWithDeliveringStatus = await _repository.GetListAsync(filters: new List<Expression<Func<Order, bool>>>
            {
                order => order.Status == OrderStatus.Delivering
            });
            var transaction = await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var order in listOrderWithDeliveringStatus)
                {
                    if (realTime > order.SessionDetail!.Session!.DeliveryEndTime)
                    {
                        order.Status = OrderStatus.Cancelled;
                        var orderActivityNumber = await _orderActivityService.CountAsync() + 1;

                        var orderActivity = new OrderActivity
                        {
                            Id = Guid.NewGuid(),
                            Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                            Name = MessageConstants.OrderActivityMessageConstrant.OrderCanceledActivityName,
                            Time = TimeUtil.GetCurrentVietNamTime(),
                            Status = OrderActivityStatus.Active
                        };
                        await _orderActivityService.CreateOrderActivityAsync(order, orderActivity, null);
                        await _repository.UpdateAsync(order);
                        await _unitOfWork.CommitAsync();
                        isUpdated = true;
                    }

                    if (!isUpdated)
                    {
                        throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionDeliveryStillAvailable);
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await transaction.RollbackAsync();
                if (ex is BeanFastApplicationException beanfastException)
                {
                    throw;
                }
            }

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
            double points = totalOrderPrice / TransactionConstrant.RewardPointDivideRate;
            return (int)Math.Round(points, MidpointRounding.AwayFromZero);
        }



        public async Task CreateOrderActivityAsync(CreateOrderActivityRequest request, User user)
        {
            request.ExchangeGiftId = null;
            if (request.OrderId == null || request.OrderId == Guid.Empty)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderIdRequired);
            }
            await _orderActivityService.CreateOrderActivityAsync(request, user);
        }

        public async Task CancelOrderAsync(User user, Guid id, CancelOrderRequest request)
        {
            var order = await GetByIdAsync(id);
            if (OrderStatus.Completed == order.Status)
            {
                throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderCompletedAlready);
            }
            if (RoleName.CUSTOMER.ToString() == user.Role!.EnglishName)
            {
                if (order.Status != OrderStatus.Cooking)
                {
                    throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderOnlyCanBeCancelIfInCookingStatus);
                }
                else
                {
                    await CancelOrderForCustomerAsync(order, request, user);
                }
            }
            else if (RoleName.MANAGER.ToString() == user.Role.EnglishName)
            {
                if (OrderStatus.Cancelled == order.Status || OrderStatus.CancelledByCustomer == order.Status)
                {
                    throw new InvalidRequestException(MessageConstants.OrderMessageConstrant.OrderAlreadyCanceled);
                }
                await CancelOrderForManagerAsync(order, request, user);
            }
        }

        public async Task CancelOrderAsync(Order order, CancelOrderRequest request, User user)
        {
            order.Status = (int)OrderStatus.Cancelled;
            var orderActivityNumber = await _orderActivityService.CountAsync() + 1;

            var orderActivity = new OrderActivity
            {
                Id = Guid.NewGuid(),
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.OrderActivityCodeConstrant.OrderActivityPrefix, orderActivityNumber),
                Name = MessageConstants.OrderActivityMessageConstrant.OrderCanceledActivityName,
                Time = TimeUtil.GetCurrentVietNamTime(),
                Status = OrderActivityStatus.Active,
                OrderId = order.Id
            };
            var transaction = await _unitOfWork.BeginTransactionAsync();

            try
            {
                await _orderActivityService.CreateOrderActivityAsync(order, orderActivity, user);
                await _repository.UpdateAsync(order);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await transaction.RollbackAsync();
            }
            await _unitOfWork.CommitAsync();
        }

        
    }
}
