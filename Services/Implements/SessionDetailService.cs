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
        private readonly ILocationService _locationService;
        private readonly IUserService _userService;
        private readonly ISessionDetailDelivererService _sessionDetailDelivererService;
        private readonly ISessionDetailRepository _repository;
        public SessionDetailService(
            IUnitOfWork<BeanFastContext> unitOfWork,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserService userService,
            ILocationService locationService,
            IUserService delivererService,
            ISessionDetailDelivererService sessionDetailDelivererService, ISessionDetailRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _userService = userService;
            _locationService = locationService;
            _userService = delivererService;
            _sessionDetailDelivererService = sessionDetailDelivererService;
            _repository = repository;
        }

        public async Task<SessionDetail> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
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
            var sessionDetails = await _repository.GetSessionDetailByDelivererIdAsync(user);
            return sessionDetails;
        }
        public async Task<ICollection<GetSessionDetailResponse>> GetIncommingDeliveringSessionDetailsAsync(User user)
        {
            return await _repository.GetIncommingDeliveringSessionDetailsAsync(user);
        }
        public async Task CreateSessionDetailAsync(CreateSessionDetailRequest request)
        {
            var sessionDetailId = Guid.NewGuid();
            var sessionDetailEntity = _mapper.Map<SessionDetail>(request);
            sessionDetailEntity.Id = sessionDetailId;
            await _locationService.GetByIdAsync(request.LocationId);
            await _userService.GetByIdAsync(request.DelivererId);
            var sessionDetailNumber = await _repository.CountAsync() + 1;
            sessionDetailEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.SessionDetailCodeConstrant.SessionDetailPrefix, sessionDetailNumber);
            sessionDetailEntity.Status = BaseEntityStatus.Active;

            await _repository.InsertAsync(sessionDetailEntity);
            await _unitOfWork.CommitAsync();
        }
        

        public async Task UpdateSessionDetailByIdAsync(
            Guid sessionDetailId, 
            UpdateSessionDetailRequest updateSessionDetailRequest, 
            List<Guid> availableDelivererIds,
            User user)
        {
            var sessionDetailEntity = await GetByIdAsync(sessionDetailId);
            var sessionDetailDeliverers = new List<SessionDetailDeliverer>();
            var sessionEntity = sessionDetailEntity.Session;
            var currentVietnamTime = TimeUtil.GetCurrentVietNamTime();
            if (currentVietnamTime.AddMinutes(TimeConstrant.NumberOfMinutesBeforeDeliveryStartTime + 1) >= sessionEntity!.DeliveryStartTime)
            {
                throw new InvalidRequestException("Không thể cập nhật thông tin giao hàng khi thời gian giao hàng sắp bắt đầu");
            }
            foreach (var item in updateSessionDetailRequest.DelivererIds.ToList())
            {
                if (!availableDelivererIds.Contains(item)) throw new InvalidRequestException(MessageConstants.SessionDetailMessageConstrant.DelivererAreBusyInAnotherSessionDetail(item));

                sessionDetailDeliverers.Add(new SessionDetailDeliverer
                {
                    DelivererId = item,
                    SessionDetailId = sessionDetailId,
                    Status = BaseEntityStatus.Active
                });
            }
            if (sessionDetailEntity.SessionDetailDeliverers != null && sessionDetailEntity.SessionDetailDeliverers.Count > 0)
            {
                await _sessionDetailDelivererService.HardDeleteAsync(sessionDetailEntity.SessionDetailDeliverers.ToList());
            }
            await _sessionDetailDelivererService.InsertListAsync(sessionDetailDeliverers);
            //sessionDetailEntity.SessionDetailDeliverers = sessionDetailDeliverers;
            //await _repository.UpdateAsync(sessionDetailEntity);
            //await _unitOfWork.CommitAsync();
        }


    }
}
