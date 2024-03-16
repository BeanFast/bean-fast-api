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


        public async Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole)
        {
            throw new NotImplementedException();
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
