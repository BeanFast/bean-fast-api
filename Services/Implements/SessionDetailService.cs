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
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using static Utilities.Constants.MessageConstants;

namespace Services.Implements
{
    public class SessionDetailService : BaseService<SessionDetail>, ISessionDetailService
    {
        private readonly IUserService _userService;

        public SessionDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IUserService userService) : base(unitOfWork, mapper, appSettings)
        {
            _userService = userService;
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

        public async Task<ICollection<GetSessionDetailResponse>> GetSessionDetailByDelivererIdAsync(Guid delivererId, Guid userId)
        {
            var delivere = await _userService.GetByIdAsync(userId);

            if (delivere.Id != delivererId)
            {
                throw new UserNotMatchException();
            }

            List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.DelivererId == delivererId
            };

            var sessionDetails = await _repository.GetListAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable
                .Include(sd => sd.Deliverer!)
                .Include(sd => sd.Location!)
                .Include(sd => sd.Session!));

            return _mapper.Map<ICollection<GetSessionDetailResponse>>(sessionDetails);
        }
    }
}
