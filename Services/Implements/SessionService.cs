using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
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

        public SessionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ILocationService locationService, ISessionDetailService sessionDetailService) : base(unitOfWork, mapper, appSettings)
        {
            _locationService = locationService;
            _sessionDetailService = sessionDetailService;
        }

        public async Task CreateSessionAsync(CreateSessionRequest request)
        {
            var sessionEntity = _mapper.Map<Session>(request);
            sessionEntity.Status = BaseEntityStatus.Active;
            sessionEntity.Id = Guid.NewGuid();
            HashSet<Guid> uniqueLocationIds = new();

            //sessionEntity.SessionDetails!.ToList().ForEach(async s =>
            //{

            //});
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
            await _repository.InsertAsync(sessionEntity);
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
            if(request.OrderStartTime != null)
            {
                filters.Add(s => s.OrderStartTime.Date.Equals(request.OrderStartTime.Value.Date));
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
                filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
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
        public async Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid guid)
        {
            throw new NotImplementedException();
        }

        public async Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id)
        {
            List<Expression<Func<Session, bool>>> filters = new()
            {
                (session) => session.Id == id && session.Status == BaseEntityStatus.Active
            };
            var result = await _repository.FirstOrDefaultAsync<GetSessionForDeliveryResponse>(filters: filters)
                 ?? throw new EntityNotFoundException(MessageConstants.SessionMessageConstrant.SessionNotFound(id));
            return result!;
        }
    }
}
