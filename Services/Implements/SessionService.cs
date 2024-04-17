using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.User.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

        private readonly ILocationService _locationService;
        private readonly ISessionDetailService _sessionDetailService;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        public SessionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ILocationService locationService, ISessionDetailService sessionDetailService, IUserService userService, IOrderService orderService) : base(unitOfWork, mapper, appSettings)
        {
            _locationService = locationService;
            _sessionDetailService = sessionDetailService;
            _userService = userService;
            _orderService = orderService;
        }

        public async Task CreateSessionAsync(CreateSessionRequest request, User user)
        {
            var sessionEntity = _mapper.Map<Session>(request);
            sessionEntity.Status = BaseEntityStatus.Active;
            sessionEntity.Id = Guid.NewGuid();
            HashSet<Guid> uniqueLocationIds = new();
            var sessionDetailNumber = await _sessionDetailService.CountAsync() + 1;

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
                if (request.Expired)
                {
                    filters.Add(s => s.OrderEndTime < currentVietNamTime);
                }
                if (request.Incomming)
                {
                    filters.Add(s => s.OrderStartTime > currentVietNamTime);
                }
                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime);
                }
            }
            else
            {
                if (request.Orderable)
                {
                    filters.Add(s => s.OrderStartTime <= currentVietNamTime && s.OrderEndTime > currentVietNamTime && s.Status == BaseEntityStatus.Active);
                }
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
            var filters = getFiltersFromSessionFilterRequest(filterRequest, userRole!);
            return await _repository.GetListAsync<GetSessionForDeliveryResponse>(filters: filters);
        }

        public async Task<Session> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Session, bool>>> filters = new()
            {
                (session) => session.Id == id
            };
            var session = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters,
                include: i => i.Include(s => s.SessionDetails!))
                ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
            return session;
        }
        public async Task<Session> GetBySessionDetailIdAsync(Guid sesionDetailId)
        {
            List<Expression<Func<Session, bool>>> filters = new()
            {
                (session) => session.SessionDetails!.Where(sd => sd.Id == sesionDetailId).Any(),
            };
            var session = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters,
                include: i => i.Include(s => s.SessionDetails!))
                ?? throw new EntityNotFoundException(MessageConstants.SessionDetailMessageConstrant.SessionDetailNotFound(sesionDetailId));
            return session;
        }

        public async Task<GetSessionForDeliveryResponse> GetSessionResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetSessionForDeliveryResponse>(await GetByIdAsync(id));
        }
        //public async Task CreateSessionAsync(CreateSessionRequest request)
        //{
        //    var session = _mapper.Map<Session>(request);
        //    await _repository.InsertAsync(session);
        //    await _unitOfWork.CommitAsync();
        //}
        public async Task<ICollection<GetDelivererResponse>> GetAvailableDelivererInSessionDeliveryTime(Guid sessionDetailId)
        {
            var session = await GetBySessionDetailIdAsync(sessionDetailId);
            ICollection<GetDelivererResponse> list = new List<GetDelivererResponse>();
            var sessions = await _repository.GetListAsync(filters: new()
            {
                s => s.DeliveryStartTime.Date == session.DeliveryStartTime.Date && s.DeliveryEndTime.Date == session.DeliveryEndTime.Date,
                s =>!(session.DeliveryStartTime < s.DeliveryEndTime && session.DeliveryEndTime > s.DeliveryStartTime),
            }, include: i => i.Include(s => s.SessionDetails!));
            var existedBusyDelivererIdList = session.SessionDetails!.Where(sd => sd.DelivererId.HasValue).Select(sd => sd.DelivererId!.Value).ToList();

            if (!sessions.IsNullOrEmpty())
            {
                var busyDelivererIds = sessions
                    .SelectMany(s => s.SessionDetails!.Where(sd => sd.DelivererId.HasValue).Select(sd => sd.DelivererId!.Value))
                    .ToList();
                // list những deliverer id mà đang có sẵn trong session detail mà người dùng chọn
                existedBusyDelivererIdList.AddRange(busyDelivererIds);
            }
            list = await _userService.GetDeliverersExcludeAsync(existedBusyDelivererIdList);

            return list;

        }
        public async Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request, User user)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid guid, User user)
        {
            var session = await _repository.FirstOrDefaultAsync(filters: new()
            {
                session => session.Id == guid,
                session => session.Status == BaseEntityStatus.Active
            }, include: i => i
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.Orders!)
                .ThenInclude(s => s.OrderActivities!)
                .Include(s => s.SessionDetails!)
                .ThenInclude(sd => sd.Orders!)
                .ThenInclude(o => o.Profile!)
                .ThenInclude(p => p.Wallets!));
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
            }
            await _repository.DeleteAsync(session, user);
            await _unitOfWork.CommitAsync();

        }

        public async Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id, SessionFilterRequest request, string? userRole)
        {
            var filters = getFiltersFromSessionFilterRequest(request, userRole!);
            filters.Add((session) => session.Id == id && session.Status == BaseEntityStatus.Active);
            var result = await _repository.FirstOrDefaultAsync<GetSessionForDeliveryResponse>(filters: filters)
                 ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
            return result!;
        }

        public async Task UpdateOrdersStatusAutoAsync()
        {
            var filters = new List<Expression<Func<Session, bool>>>()
            {
               session => session.Status != SessionStatus.Deleted && session.Status != SessionStatus.Ended
            };
            var sessions = await _repository
                .GetListAsync(filters: filters, include: i => i.Include(s => s.SessionDetails!).ThenInclude(sd => sd.Orders!));
            foreach (var s in sessions)
            {
                if (s.Status == SessionStatus.Active || s.Status == SessionStatus.Incoming)
                {

                    if (s.Status == SessionStatus.Active)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
                        if (currentTime.AddMinutes(TimeConstrant.NumberOfMinutesBeforeDeliveryStartTime) >= s.DeliveryStartTime)
                        {
                            CancelOrderRequest request = new()
                            {
                                Reason = "Đơn hàng bị hủy do chưa có người giao"
                            };
                            foreach (var sd in s.SessionDetails)
                            {
                                if (sd.DelivererId == null)
                                {
                                    foreach (var order in sd.Orders)
                                    {
                                        var orderIncludeWallet = await _orderService.GetByIdAsync(order.Id);
                                        await _orderService.CancelOrderForManagerAsync(orderIncludeWallet, request, null!);
                                    }

                                }
                                else
                                {

                                    foreach (var order in sd.Orders)
                                    {
                                        if (order.Status == OrderStatus.Cooking)
                                        {
                                            await _orderService.UpdateOrderDeliveryStatusAsync(order.Id);
                                        }
                                    }
                                }
                                sd.Orders = null;
                            }
                            s.Status = SessionStatus.Incoming;
                        }
                        await _repository.UpdateAsync(s);
                        await _unitOfWork.CommitAsync();
                    }
                    else if (s.Status == SessionStatus.Incoming)
                    {
                        var currentTime = TimeUtil.GetCurrentVietNamTime();
                        if (currentTime > s.DeliveryEndTime)
                        {
                            CancelOrderRequest request = new()
                            {
                                Reason = "Đơn hàng bị hủy do chưa có người nhận"
                            };
                            foreach (var sd in s.SessionDetails!)
                            {
                                foreach (var order in sd.Orders!)
                                {
                                    if (order.Status == OrderStatus.Delivering)
                                    {
                                        var orderIncludeWallet = await _orderService.GetByIdAsync(order.Id);
                                        await _orderService.CancelOrderForCustomerAsync(order, request, null!);
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
    }
}
