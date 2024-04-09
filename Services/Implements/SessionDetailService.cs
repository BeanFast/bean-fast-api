using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.SessionDetail.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class SessionDetailService : BaseService<SessionDetail>, ISessionDetailService
    {
        private readonly IUserService _userService;
        private readonly ILocationService _locationService;
        private readonly IUserService _delivererService;

        public SessionDetailService(
            IUnitOfWork<BeanFastContext> unitOfWork,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserService userService,
            ILocationService locationService,
            IUserService delivererService) : base(unitOfWork, mapper, appSettings)
        {
            _userService = userService;
            _locationService = locationService;
            _delivererService = delivererService;
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
        public async Task<ICollection<GetSessionDetailResponse>> GetSessionDetailsAsync(User user, GetSessionDetailFilterRequest filterReqeuest)
        {
            if (RoleName.MANAGER.ToString() == user.Role!.EnglishName)
            {
                //    if (!filterReqeuest.Status.IsNullOrEmpty() && filterReqeuest.Status!.ToLower() == "incoming")
                //{
                //if (RoleName.MANAGER.ToString() == user.Role!.EnglishName) throw new InvalidRoleException();
                return await GetIncommingDeliveringSessionDetailsAsync(user);
            }
            else
            {
                return await GetSessionDetailByDelivererIdAsync(user);
            }
        }
        public async Task<ICollection<GetSessionDetailResponse>> GetSessionDetailByDelivererIdAsync(User user)
        {

            List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.DelivererId == user.Id,
                (sessionDetail) => sessionDetail.Status == BaseEntityStatus.Active,
            };

            var sessionDetails = await _repository.GetListAsync<GetSessionDetailResponse>(
                filters: filters
                //, include: queryable => queryable
                //.Include(sd => sd.Orders!).ThenInclude(o => o.OrderDetails!)
                //.Include(sd => sd.Location!).ThenInclude(l => l.School!).ThenInclude(s => s.Area!)
                //.Include(sd => sd.Session!)
                );

            return sessionDetails;
        }
        public async Task<ICollection<GetSessionDetailResponse>> GetIncommingDeliveringSessionDetailsAsync(User user)
        {
            List<Expression<Func<SessionDetail, bool>>> filters = new()
            {
                (sessionDetail) => sessionDetail.Session!.DeliveryStartTime > TimeUtil.GetCurrentVietNamTime()
            };
            var sessionDetails = await _repository.GetListAsync<GetSessionDetailResponse>(status: BaseEntityStatus.Active,
                filters: filters);
            return sessionDetails;
        }
        public async Task CreateSessionDetailAsync(CreateSessionDetailRequest request)
        {
            var sessionDetailId = Guid.NewGuid();
            var sessionDetailEntity = _mapper.Map<SessionDetail>(request);
            sessionDetailEntity.Id = sessionDetailId;
            await _locationService.GetByIdAsync(request.LocationId);
            await _delivererService.GetByIdAsync(request.DelivererId);
            var sessionDetailNumber = await _repository.CountAsync() + 1;
            sessionDetailEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SessionDetailCodeConstrant.SessionDetailPrefix, sessionDetailNumber);
            sessionDetailEntity.Status = BaseEntityStatus.Active;

            await _repository.InsertAsync(sessionDetailEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateSessionDetailByIdAsync(Guid sessionDetailId, UpdateSessionDetailRequest updateSessionDetailRequest)
        {
            var sessionDetailEntity = await GetByIdAsync(sessionDetailId);
            await _userService.GetByIdAsync(updateSessionDetailRequest.DelivererId);
            sessionDetailEntity.DelivererId = updateSessionDetailRequest.DelivererId;

            await _repository.UpdateAsync(sessionDetailEntity);
            await _unitOfWork.CommitAsync();
        }


    }
}
