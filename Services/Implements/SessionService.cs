﻿using AutoMapper;
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
            foreach (var item in sessionsHasDeliveryTimeOverlap)
            {
                
                var matchingSessionDetail = request.SessionDetails.FirstOrDefault(rsd => item.SessionDetails!.Any(sd => rsd.LocationId == sd.LocationId));

                if (matchingSessionDetail != null)
                {
                    throw new InvalidRequestException(MessageConstants.SessionMessageConstrant.OverlappedSessionHasExistedLocationId(item.DeliveryStartTime, item.DeliveryEndTime, matchingSessionDetail.LocationId));
                }
                
            }
            foreach (var sessionDetail in sessionEntity.SessionDetails!)
            {
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
                }
            }
            var sessionNumber = await _repository.CountAsync() + 1;
            sessionEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SessionCodeConstrant.SessionPrefix, sessionNumber);
            await _repository.InsertAsync(sessionEntity, user);
            await _unitOfWork.CommitAsync();
        }

        private List<Expression<Func<Session, bool>>> getFiltersFromSessionFilterRequest(SessionFilterRequest request, string userRole)
        {
            List<Expression<Func<Session, bool>>> filters = new List<Expression<Func<Session, bool>>>();
            //Expression<Func<Session, bool>> orFilter = (s) =>
            //{
            //    bool filter = false;
            //    if (request.Expired)
            //    {
            //        filter = filter || s.OrderEndTime < TimeUtil.GetCurrentVietNamTime();
            //    }
            //    if (request.Incomming)
            //    {
            //        filter = filter || s.OrderStartTime > TimeUtil.GetCurrentVietNamTime();
            //    }
            //    if (request.Orderable)
            //    {
            //        filter = filter || (s.OrderStartTime <= TimeUtil.GetCurrentVietNamTime() && s.OrderEndTime > TimeUtil.GetCurrentVietNamTime());
            //    }
            //    return filter;
            //};
            //filters.Add(orFilter);
            var currentVietNamTime = TimeUtil.GetCurrentVietNamTime();
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                
                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime);
                }
            }
            else
            {
                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime);
                }
                filters.Add(s => s.Status != BaseEntityStatus.Deleted);
            }
            if (request.Expired)
            {
                filters.Add(s => s.OrderEndTime < currentVietNamTime);
            }
            if (request.Incomming)
            {
                filters.Add(s => s.OrderStartTime > currentVietNamTime);
            }
            if (request.MenuId != Guid.Empty)
            {
                filters.Add(s => s.MenuId == request.MenuId);
            }
            if (request.SchoolId != null && request.SchoolId != Guid.Empty)
            {
                filters.Add(s => s.SessionDetails!.Where(sd => sd.Location!.SchoolId == request.SchoolId && sd.Status == BaseEntityStatus.Active).Any());
            }
            if (request.OrderStartTime != null)
            {
                filters.Add(s => s.OrderStartTime.Date.Equals(request.OrderStartTime.Value.Date));
            }
            if (request.OrderTime != null)
            {
                filters.Add(s => s.OrderStartTime.Date <= request.OrderTime.Value.Date && s.OrderEndTime.Date >= request.OrderTime.Value.Date);
            }
            if (request.DeliveryTime != null)
            {
                filters.Add(s => s.DeliveryStartTime.Date <= request.DeliveryTime.Value.Date && s.DeliveryEndTime.Date >= request.DeliveryTime.Value.Date);
            }
            //if(request.DeliveryEndTime)
            return filters;
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
            var existedBusyDelivererIdList = session.SessionDetails?.SelectMany(sd =>
            {
                return sd.SessionDetailDeliverers!.Select(sdd => sdd.DelivererId);
            }).ToList();

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
                if (s.Status == SessionStatus.Active || s.Status == SessionStatus.Incoming)
                {

                    if (s.Status == SessionStatus.Active)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
                        if (currentTime.AddMinutes(TimeConstrant.NumberOfMinutesBeforeDeliveryStartTime) >= s.DeliveryStartTime)
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
                                        if (order.Status == OrderStatus.Cooking)
                                        {
                                            await _orderService.UpdateOrderDeliveryStatusAsync(order.Id);
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
                            await _repository.UpdateAsync(s);
                            await _unitOfWork.CommitAsync();
                        }

                    }
                    else if (s.Status == SessionStatus.Incoming)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
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
                                        //await _exchangeGIftService.CancelExchangeGiftForCustomerAsync(exchangeGift, cancelExchangeGiftRequest, null!);
                                    }
                                }
                            }
                            s.Status = SessionStatus.Ended;
                            await _repository.UpdateAsync(s);
                            await _unitOfWork.CommitAsync();
                        }
                    }

                }
            }
        }

        public async Task UpdateSessionDetailByIdAsync(Guid sessionDetailId, UpdateSessionDetailRequest request)
        {
            var availableDeliverers = await GetAvailableDelivererInSessionDeliveryTime(sessionDetailId);
            await _sessionDetailService.UpdateSessionDetailByIdAsync(sessionDetailId, request, availableDeliverers.Select(d => d.Id).ToList());
        }
        
    }
}
