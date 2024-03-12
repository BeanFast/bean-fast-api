using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Profiles;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Settings;
using Utilities.Utils;

namespace Services.Implements
{
    public class ProfileService : BaseService<Profile>, IProfileService
    {

        private readonly ISchoolService _schoolService;

        private readonly ICloudStorageService _cloudStorageService;
        public ProfileService(IUnitOfWork<BeanFastContext> unitOfWork, AutoMapper.IMapper mapper, IOptions<AppSettings> appSettings, ISchoolService schoolService, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper, appSettings)
        {
            _schoolService = schoolService;
            _cloudStorageService = cloudStorageService;
        }

        public async Task CreateProfileAsync(CreateProfileRequest request, Guid userId)
        {
            var profileEntity = _mapper.Map<Profile>(request);
            var currentBMI = _mapper.Map<ProfileBodyMassIndex>(request.Bmi);
            
            currentBMI.Id = Guid.NewGuid();
            var profileId = Guid.NewGuid();
            profileEntity.UserId = userId;
            profileEntity.Code = EntityCodeUtil.GenerateNamedEntityCode(EntityCodeConstrant.ProfileCodeConstrant.ProfilePrefix, profileEntity.FullName, profileId); ;
            currentBMI.ProfileId = profileId;
            
            var school = await _schoolService.GetSchoolByIdAsync(request.SchoolId);
            var imagePath = await _cloudStorageService.UploadFileAsync(profileId, _appSettings.Firebase.FolderNames.Profile, request.Image);
            profileEntity.AvatarPath = imagePath;
            profileEntity.Id = profileId;
            profileEntity.Status = Utilities.Statuses.BaseEntityStatus.Active;
            profileEntity.CurrentBMI = currentBMI.Weight / (currentBMI.Height * currentBMI.Height);

            await _repository.InsertAsync(profileEntity);
            await _unitOfWork.GetRepository<ProfileBodyMassIndex>().InsertAsync(currentBMI);
            await _unitOfWork.CommitAsync();
        }
    }
}
