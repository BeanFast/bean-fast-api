using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.SessionDetail.Response;
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

namespace Services.Implements
{
    public class SessionDetailService : BaseService<SessionDetail>, ISessionDetailService
    {
        public SessionDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<SessionDetail> GetByIdAsync(Guid id)
        {
            List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.Id == id
            };
            var sessionDetail = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(sd => sd.Deliverer!).Include(sd => sd.Location!).Include(sd => sd.Session!))
                ?? throw new EntityNotFoundException(MessageConstants.SessionDetailMessageConstrant.SessionDetailNotFound(id));
            return sessionDetail;
        }

        public async Task<GetSessionDetailResponse> GetSessionDetailResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetSessionDetailResponse>(await GetByIdAsync(id));
        }
    }
}
