using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
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

namespace Services.Implements
{
    public class SessionService : BaseService<Session>, ISessionService
    {
        public SessionService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task CreateSessionAsync(CreateSessionRequest request)
        {
            throw new NotImplementedException();
        }

        private List<Expression<Func<Session, bool>>> getFiltersFromSessionFilterRequest(SessionFilterRequest request)
        {
            List<Expression<Func<Session, bool>>> filters = new List<Expression<Func<Session, bool>>>();
            if(request.Orderable)
            {
                filters.Add((s) => s.OrderStartTime > DateTime.Now && s.OrderEndTime < DateTime.Now);
                filters.Add((s) => s.Status == BaseEntityStatus.Active);
            }
            if(request.MenuId != Guid.Empty)
            {
                filters.Add(s => s.MenuId == request.MenuId);
            }
           
            //if(request.DeliveryEndTime)
            return filters;
        }
        public async Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest)
        {
            var filters = getFiltersFromSessionFilterRequest(filterRequest);
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

        public async Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
