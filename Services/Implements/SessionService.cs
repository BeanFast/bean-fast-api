using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.User.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;
namespace Services.Implements
{
    public class SessionService : BaseService<Session>, ISessionService
    {
        private readonly ISessionRepository _repository;
        private readonly ILocationService _locationService;
        private readonly ISessionDetailService _sessionDetailService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IExchangeGIftService _exchangeGIftService;
        private readonly IMenuService _menuService;
        public SessionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ILocationService locationService, ISessionDetailService sessionDetailService, IUserService userService, IOrderService orderService, IExchangeGIftService exchangeGIftService, ISessionRepository repository, IMenuService menuService) : base(unitOfWork, mapper, appSettings)
        {
            _locationService = locationService;
            _sessionDetailService = sessionDetailService;
            _userService = userService;
            _orderService = orderService;
            _exchangeGIftService = exchangeGIftService;
            _repository = repository;
            _menuService = menuService;
        }

        public async Task CreateSessionAsync(CreateSessionRequest request, User user)
        {
            var sessionEntity = _mapper.Map<Session>(request);
            sessionEntity.Status = BaseEntityStatus.Active;
            sessionEntity.Id = Guid.NewGuid();
            HashSet<Guid> uniqueLocationIds = new();
            var menu = await _menuService.GetByIdAsync(request.MenuId);
            var sessionDetailNumber = await _sessionDetailService.CountAsync() + 1;
            var sessionsHasDeliveryTimeOverlap = await _repository.GetListAsync(filters: new()
            {
                s => s.DeliveryStartTime < sessionEntity.DeliveryEndTime && s.DeliveryEndTime > sessionEntity.DeliveryStartTime
            }, include: i => i.Include(s => s.SessionDetails!));
            var availableDeliverers = await GetAvailableDelivererInSessionDeliveryTime(sessionEntity.DeliveryStartTime, sessionEntity.DeliveryEndTime);
            foreach (var item in sessionsHasDeliveryTimeOverlap)
            {

                var matchingSessionDetail = request.SessionDetails.FirstOrDefault(rsd => item.SessionDetails!.Any(sd => rsd.LocationId == sd.LocationId));

                if (matchingSessionDetail != null)
                {
                    throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.OverlappedSessionHasExistedLocationId(item.DeliveryStartTime, item.DeliveryEndTime, matchingSessionDetail.LocationId));
                }
            }

            for (int i = 0; i < sessionEntity.SessionDetails.Count; i++)
            {
                var sessionDetail = sessionEntity.SessionDetails.ElementAt(i);
                sessionDetail!.SessionDetailDeliverers = new List<SessionDetailDeliverer>();
                if (uniqueLocationIds.Contains(sessionDetail.LocationId))
                {
                    throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.DuplicateLocationInSession);
                }
                else
                {
                    await _locationService.GetByIdAsync(sessionDetail.LocationId);
                    sessionDetail.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SessionDetailCodeConstrant.SessionDetailPrefix, sessionDetailNumber);
                    sessionDetail.Status = BaseEntityStatus.Active;
                    sessionDetailNumber++;
                    uniqueLocationIds.Add(sessionDetail.Id);
                    foreach (var sessionDetailDeliverer in request.SessionDetails.ElementAt(i).Deliverers)
                    {
                        if (availableDeliverers.Any(ad => ad.Id != sessionDetailDeliverer.DelivererId))
                        {
                            availableDeliverers.Remove(availableDeliverers.First(ad => ad.Id == sessionDetailDeliverer.DelivererId));
                            sessionDetail!.SessionDetailDeliverers!.Add(new SessionDetailDeliverer
                            {
                                DelivererId = sessionDetailDeliverer.DelivererId,
                                SessionDetailId = sessionDetail.Id,
                                CreatedDate = TimeUtil.GetCurrentVietNamTime(),
                                UpdatedDate = TimeUtil.GetCurrentVietNamTime(),
                                Status = BaseEntityStatus.Active,
                                CreatorId = user.Id,
                                UpdaterId = user.Id
                            });
                        }
                        else
                        {
                            throw new InvalidRequestException("Deliverer is not available in delivery time");
                        }
                    }
                }
            }
            var sessionNumber = await _repository.CountAsync() + 1;
            sessionEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SessionCodeConstrant.SessionPrefix, sessionNumber);
            await _repository.InsertAsync(sessionEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest)
        {
            return await _repository.GetAllAsync(userRole, filterRequest);
        }

        public async Task<Session> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<Session> GetBySessionDetailIdAsync(Guid sesionDetailId)
        {
            return await _repository.GetBySessionDetailIdAsync(sesionDetailId);
        }

        public async Task<GetSessionForDeliveryResponse> GetSessionResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetSessionForDeliveryResponse>(await GetByIdAsync(id));
        }
        public async Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id, SessionFilterRequest request, string? userRole)
        {
            return await _repository.GetSessionForDeliveryResponseByIdAsync(id, request, userRole);
        }

        public async Task<bool> CheckOrderable(Guid menuDetailId, Guid profileId, Guid sessionId)
        {
            var session = await _repository.GetSessionByMenuDetailIdAndProfileIdAndSessionIdAsync(menuDetailId, profileId, sessionId);
            if (session == null || session.Menu == null || session.SessionDetails!.Count == 0 || session.Menu!.MenuDetails!.Count == 0)
            {
                return false;
            }
            bool profileIdIsInSchoolOfSession = false;
            bool menuDetailIdIsInMenuOfSession = false;
            var currentVietnamTime = TimeUtil.GetCurrentVietNamTime();
            if (session.OrderStartTime < currentVietnamTime || session.OrderEndTime > currentVietnamTime)
                return false;
            foreach (var sessionDetail in session.SessionDetails!)
            {
                if (sessionDetail.Location!.School!.Profiles!.Any(p => p.Id == profileId))
                {
                    profileIdIsInSchoolOfSession = true;
                }
            }
            foreach (var menuDetail in session.Menu.MenuDetails)
            {
                if (menuDetail.Id == menuDetailId)
                {
                    menuDetailIdIsInMenuOfSession = true;
                }
            }
            return menuDetailIdIsInMenuOfSession && profileIdIsInSchoolOfSession;
        }

        public async Task<ICollection<GetDelivererResponse>> GetAvailableDelivererInSessionDeliveryTime(Guid sessionDetailId)
        {
            ICollection<GetDelivererResponse> list = new List<GetDelivererResponse>();

            var session = await GetBySessionDetailIdAsync(sessionDetailId);
            var sessions = await _repository.GetOverlappedDeliveryTimeSessions(session.DeliveryStartTime, session.DeliveryEndTime);
            sessions = sessions.Where(s => s.Id != session.Id).ToList();
            var existedBusyDelivererIdList = new List<Guid>();
            foreach (var sd in session.SessionDetails!)
            {
                if (sd.Id != sessionDetailId)
                {
                    existedBusyDelivererIdList.AddRange(sd.SessionDetailDeliverers.Select(s => s.DelivererId));
                }
            }
            if (!sessions.IsNullOrEmpty())
            {
                var busyDelivererIds = sessions
                    .SelectMany(s => s.SessionDetails!.SelectMany(sd => sd.SessionDetailDeliverers!.Select(sdd => sdd.DelivererId)))
                    .ToList();
                // list những deliverer id mà đang có sẵn trong session detail mà người dùng chọn
                existedBusyDelivererIdList?.AddRange(busyDelivererIds);
            }
            list = await _userService.GetDeliverersExcludeAsync(existedBusyDelivererIdList!.ToList());

            return list;

        }
        public async Task<ICollection<GetDelivererResponse>> GetAvailableDelivererInSessionDeliveryTime(DateTime deliveryStartTime, DateTime deliveryEndTime)
        {
            ICollection<GetDelivererResponse> list = new List<GetDelivererResponse>();

            var sessions = await _repository.GetOverlappedDeliveryTimeSessions(deliveryStartTime, deliveryEndTime);
            var busyDelivererIdList = new List<Guid>();

            if (!sessions.IsNullOrEmpty())
            {
                busyDelivererIdList = sessions
                    .SelectMany(s => s.SessionDetails!.SelectMany(sd => sd.SessionDetailDeliverers!.Select(sdd => sdd.DelivererId)))
                    .ToList();
                // list những deliverer id mà đang có sẵn trong session detail mà người dùng chọn

            }
            list = await _userService.GetDeliverersExcludeAsync(busyDelivererIdList);

            return list;

        }
        public async Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request, User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid guid, User user)
        {
            var session = await _repository.GetByIdForDelete(guid);
            if (session == null) throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionNotFound(guid));
            var currentVietNamTime = TimeUtil.GetCurrentVietNamTime();

            if (session.DeliveryStartTime >= currentVietNamTime && session.DeliveryEndTime <= currentVietNamTime)
                throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionDeliveryStillAvailable);
            foreach (var sessionDetail in session.SessionDetails!)
            {
                var orders = sessionDetail.Orders;
                if (!orders.IsNullOrEmpty())
                {
                    foreach (var order in orders)
                    {
                        var cancelOrderRequest = new CancelOrderRequest { Reason = "Đơn hàng bị hủy vì quản lý xóa phiên giao hàng" };
                        await _orderService.CancelOrderAsync(order, cancelOrderRequest, user);
                    }
                }
                sessionDetail.Status = BaseEntityStatus.Deleted;
                sessionDetail.Orders = null;
            }
            await _repository.DeleteAsync(session, user);
            await _unitOfWork.CommitAsync();

        }


        public async Task UpdateOrdersStatusAutoAsync()
        {
            var filters = new List<Expression<Func<Session, bool>>>()
            {
               session => session.Status != SessionStatus.Deleted && session.Status != SessionStatus.Ended
            };
            var sessions = await _repository
                .GetListAsync(filters: filters,
                include: i => i
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.SessionDetailDeliverers!)
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.Orders!)
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.ExchangeGifts!));
            foreach (var s in sessions)
            {
                await Console.Out.WriteLineAsync(s.Code);
                if (s.Status == SessionStatus.Active || s.Status == SessionStatus.Incoming)
                {

                    if (s.Status == SessionStatus.Active)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
                        if (currentTime >= s.OrderEndTime && currentTime < s.DeliveryStartTime)
                        {
                            CancelOrderRequest cancelOrderRequest = new()
                            {
                                Reason = MessageConstants.OrderMessageConstrant.NoDeliverer
                            };
                            CancelExchangeGiftRequest cancelExchangeGiftRequest = new()
                            {
                                Reason = MessageConstants.OrderMessageConstrant.NoDeliverer
                            };
                            foreach (var sd in s.SessionDetails!)
                            {
                                if (sd.SessionDetailDeliverers!.Count() == 0)
                                {
                                    foreach (var order in sd.Orders!)
                                    {
                                        var orderIncludeWallet = await _orderService.GetByIdAsync(order.Id);
                                        await _orderService.CancelOrderForManagerAsync(orderIncludeWallet, cancelOrderRequest, null!);
                                    }
                                    foreach (var exchangeGift in sd.ExchangeGifts!)
                                    {
                                        var exchangeGiftIncludeWallet = await _exchangeGIftService.GetByIdAsync(exchangeGift.Id);
                                        await _exchangeGIftService.CancelExchangeGiftForManagerAsync(exchangeGiftIncludeWallet, cancelExchangeGiftRequest, null!);
                                    }
                                }
                                else
                                {

                                    foreach (var order in sd.Orders!)
                                    {
                                        if (order.Status == OrderStatus.Pending)
                                        {
                                            await _orderService.UpdateOrderCookingStatusAsync(order.Id);
                                        }
                                    }
                                    foreach (var exchangeGift in sd.ExchangeGifts!)
                                    {
                                        if (exchangeGift.Status == ExchangeGiftStatus.Active)
                                        {
                                            await _exchangeGIftService.UpdateExchangeGiftToDeliveryStatusAsync(exchangeGift.Id);
                                        }
                                    }
                                }
                                sd.Orders = null;
                                sd.ExchangeGifts = null;
                            }
                            s.Status = SessionStatus.Incoming;
                            s.SessionDetails = null;
                            await _repository.UpdateAsync(s);
                            await _unitOfWork.CommitAsync();
                        }

                    }
                    else if (s.Status == SessionStatus.Incoming)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
                        // nếu hết thời gian giao hàng nhưng người dùng chưa nhận hàng
                        if (currentTime > s.DeliveryEndTime)
                        {
                            CancelOrderRequest request = new()
                            {
                                Reason = MessageConstants.OrderMessageConstrant.NoReceiver
                            };
                            CancelExchangeGiftRequest cancelExchangeGiftRequest = new()
                            {
                                Reason = MessageConstants.OrderMessageConstrant.NoReceiver
                            };
                            foreach (var sd in s.SessionDetails!)
                            {
                                foreach (var order in sd.Orders!)
                                {
                                    if (order.Status == OrderStatus.Delivering)
                                    {
                                        var orderIncludeWallet = await _orderService.GetByIdAsync(order.Id);
                                        await _orderService.CancelOrderForCustomerAsync(orderIncludeWallet, request, orderIncludeWallet!.Profile!.User!);
                                    }
                                }
                                foreach (var eg in sd.ExchangeGifts!)
                                {
                                    if (eg.Status == ExchangeGiftStatus.Delivering)
                                    {
                                        var exchangeGift = await _exchangeGIftService.GetByIdAsync(eg.Id);
                                        await _exchangeGIftService.CancelExchangeGiftForCustomerAsync(exchangeGift, cancelExchangeGiftRequest, exchangeGift.Profile!.User!);
                                    }
                                }
                            }
                            s.Status = SessionStatus.Ended;
                            s.SessionDetails = null;
                            await _repository.UpdateAsync(s);
                            await _unitOfWork.CommitAsync();
                        }
                        else if (currentTime >= s.DeliveryStartTime && currentTime < s.DeliveryEndTime)
                        {
                            foreach (var sd in s.SessionDetails!)
                            {
                                foreach (var order in sd.Orders!)
                                {
                                    if (order.Status == OrderStatus.Cooking)
                                    {
                                        await _orderService.UpdateOrderDeliveryStatusAsync(order.Id);
                                    }
                                }
                                //foreach (var eg in sd.ExchangeGifts!)
                                //{
                                //    if (eg.Status == ExchangeGiftStatus.Delivering)
                                //    {
                                //        var exchangeGift = await _exchangeGIftService.GetByIdAsync(eg.Id);
                                //        //await _exchangeGIftService.CancelExchangeGiftForCustomerAsync(exchangeGift, cancelExchangeGiftRequest, null!);
                                //    }
                                //}
                            }
                        }
                    }

                }
            }
        }

        public async Task UpdateSessionDetailByIdAsync(Guid sessionDetailId, UpdateSessionDetailRequest request, User manager)
        {
            var currentSession = await GetBySessionDetailIdAsync(sessionDetailId);
            if (currentSession == null)
            {
                throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionDetailIdIsNotExisted);
            }
            if (currentSession.DeliveryStartTime <= TimeUtil.GetCurrentVietNamTime())
            {
                throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.SessionIsDelivering);
            }
            var availableDeliverers = await GetAvailableDelivererInSessionDeliveryTime(sessionDetailId);
            await _sessionDetailService.UpdateSessionDetailByIdAsync(sessionDetailId, request, availableDeliverers.Select(d => d.Id).ToList(), manager);
        }

    }
}
