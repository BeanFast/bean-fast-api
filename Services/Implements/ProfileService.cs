using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.Profiles.Request;
using DataTransferObjects.Models.Profiles.Response;
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
    public class ProfileService : BaseService<Profile>, IProfileService
    {

        private readonly ISchoolService _schoolService;
        //private readonly IWalletService _wallletService;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly IProfileRepository _repository;
        public ProfileService(IUnitOfWork<BeanFastContext> unitOfWork, AutoMapper.IMapper mapper, IOptions<AppSettings> appSettings, ISchoolService schoolService, ICloudStorageService cloudStorageService
            //, IWalletService wallletService
            , IProfileRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _schoolService = schoolService;
            _cloudStorageService = cloudStorageService;
            //_wallletService = wallletService;
            _repository = repository;
        }

        
        public async Task<Profile> GetProfileByIdAsync(int status, Guid id)
        {
            return await _repository.GetProfileByIdAsync(status, id);
        }
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            return await _repository.GetProfileByIdAsync(id);
        }
        
        public async Task<Profile> GetProfileByIdForUpdateProfileAsync(Guid id)
        {
            return await _repository.GetProfileByIdForUpdateProfileAsync(id);
        }
        
        public async Task<ICollection<GetProfileResponse>> GetProfilesByCustomerIdAsync(Guid customerId)
        {
            return await _repository.GetProfilesByCustomerIdAsync(customerId);
        }

        public async Task<Profile> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<GetProfileResponse> GetProfileResponseByIdAsync(Guid id, User user)
        {
            return await _repository.GetProfileResponseByIdAsync(id, user);
        }

        public async Task<Profile> GetProfileByIdAndCurrentCustomerIdAsync(Guid profileId, Guid customerId)
        {
            return await _repository.GetProfileByIdAndCurrentCustomerIdAsync(profileId, customerId);
        }
        public async Task CreateProfileAsync(CreateProfileRequest request, Guid userId, User user)
        {
            var profileEntity = _mapper.Map<Profile>(request);
            var currentBMI = _mapper.Map<ProfileBodyMassIndex>(request.Bmi);

            currentBMI.Id = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            profileEntity.UserId = userId;
            var profileNumber = await _repository.CountAsync() + 1;
            profileEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.ProfileCodeConstrant.ProfilePrefix, profileNumber);
            currentBMI.ProfileId = profileId;

            var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            var imagePath = await _cloudStorageService.UploadFileAsync(profileId, _appSettings.Firebase.FolderNames.Profile, request.Image);
            profileEntity.AvatarPath = imagePath;
            profileEntity.Id = profileId;
            profileEntity.Status = BaseEntityStatus.Active;
            profileEntity.CurrentBMI = currentBMI.Weight / (currentBMI.Height * currentBMI.Height);
            currentBMI.RecordDate = TimeUtil.GetCurrentVietNamTime();
            var pointsWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Name = profileEntity.FullName,
                UserId = userId,
            };
            await _repository.InsertAsync(profileEntity, user);
            await _unitOfWork.GetRepository<ProfileBodyMassIndex>().InsertAsync(currentBMI);
            await _unitOfWork.CommitAsync();
            //await _wallletService.CreateWalletAsync(WalletType.Points, pointsWallet);
        }

        public async Task UpdateProfileAsync(Guid id, UpdateProfileRequest request, User user)
        {
            var profile = await GetProfileByIdForUpdateProfileAsync(id);
            await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            if (profile.UserId != user.Id)
            {
                throw new InvalidRequestException(MessageConstants.ProfileMessageConstrant.ProfileDoesNotBelongToUser);
            }
            profile.NickName = request.NickName;
            profile.Class = request.Class;
            profile.Gender = request.Gender;
            profile.Dob = request.Dob;
            profile.FullName = request.FullName;
            if (request.Image != null)
            {
                var imagePath = await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.Profile, request.Image);
                profile.AvatarPath = imagePath;
            }
            var currentBMI = _mapper.Map<ProfileBodyMassIndex>(request.Bmi);
            currentBMI.RecordDate = TimeUtil.GetCurrentVietNamTime();
            profile.CurrentBMI = currentBMI.Weight / (currentBMI.Height * currentBMI.Height);
            profile.BMIs!.Add(currentBMI);
            await _repository.UpdateAsync(profile, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteProfileAsync(Guid id)
        {
            var profile = await GetProfileByIdAsync(BaseEntityStatus.Active, id);
            await _repository.DeleteAsync(profile);
            await _unitOfWork.CommitAsync();
        }
    }
}
