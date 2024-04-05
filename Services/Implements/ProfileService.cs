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
        private readonly IWalletService _wallletService;
        private readonly ICloudStorageService _cloudStorageService;
        public ProfileService(IUnitOfWork<BeanFastContext> unitOfWork, AutoMapper.IMapper mapper, IOptions<AppSettings> appSettings, ISchoolService schoolService, ICloudStorageService cloudStorageService, IWalletService wallletService) : base(unitOfWork, mapper, appSettings)
        {
            _schoolService = schoolService;
            _cloudStorageService = cloudStorageService;
            _wallletService = wallletService;
        }

        public async Task CreateProfileAsync(CreateProfileRequest request, Guid userId)
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
            profileEntity.Status = Utilities.Statuses.BaseEntityStatus.Active;
            profileEntity.CurrentBMI = currentBMI.Weight / (currentBMI.Height * currentBMI.Height);
            
            var pointsWallet = new Wallet
            {
                Id = Guid.NewGuid(),
                ProfileId = profileId,
                Name = profileEntity.FullName,
                UserId = userId,
            };
            await _repository.InsertAsync(profileEntity);
            await _unitOfWork.GetRepository<ProfileBodyMassIndex>().InsertAsync(currentBMI);
            await _unitOfWork.CommitAsync();
            await _wallletService.CreateWalletAsync(WalletType.Points, pointsWallet);
        }
        public async Task<Profile> GetProfileByIdAsync(int status, Guid id)
        {
            var profile = await _repository.FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id,
                s => s.Status == status
            }) ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
            return profile;
        }
        public async Task<Profile> GetProfileByIdAsync(Guid id)
        {
            var profile = await _repository.FirstOrDefaultAsync(filters: new()
            {
                s => s.Id == id
            }) ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
            return profile;
        }
        public async Task DeleteProfileAsync(Guid id)
        {
            var profile = await GetProfileByIdAsync(BaseEntityStatus.Active, id);
            await _repository.DeleteAsync(profile);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateProfileAsync(Guid id, UpdateProfileRequest request)
        {
            var profile = await GetProfileByIdAsync(BaseEntityStatus.Active, id);
            await _repository.UpdateAsync(profile);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<GetProfilesByCurrentCustomerResponse>> GetProfilesByCustomerIdAsync(Guid customerId)
        {
            var profiles = await _repository.GetListAsync<GetProfilesByCurrentCustomerResponse>(BaseEntityStatus.Active,
                filters: new()
                {
                    p => p.UserId == customerId,
                });
            //foreach (var profile in profiles)
            //{
            //    profile.SchoolName = profile.Schoo
            //}
            return profiles;
        }

        public async Task<Profile> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Profile, bool>>> filters = new()
            {
                (profile) => profile.Id == id
            };
            var profile = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable
                .Include(p => p.School!)
                .ThenInclude(s => s.Kitchen)
                .ThenInclude(k => k.Menus!.Where(m => m.Status == BaseEntityStatus.Active))
                .ThenInclude(m => m.MenuDetails!.Where(md => md.Status == BaseEntityStatus.Active)))
                ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
            return profile;
        }
        

        public async Task<GetProfileResponse> GetProfileResponseByIdAsync(Guid id, User user)
        {
            List<Expression<Func<Profile, bool>>> filters = new()
            {
                (profile) => profile.Id == id
            };
            if (RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName))
            {
                filters.Add(p => p.UserId == user.Id);
            }
            
            var profile = await _repository.FirstOrDefaultAsync<GetProfileResponse>(
                status: BaseEntityStatus.Active,
                filters: filters, 
                include: queryable => queryable
                    .Include(p => p.School!)
                    .Include(p => p.LoyaltyCards!.Where(lc => lc.Status == BaseEntityStatus.Active))
                    .Include(p => p.Wallets!
                            .Where(w => w.Status == BaseEntityStatus.Active 
                                && WalletType.Points.ToString().Equals(w.Type)
                             )
                        )
                )
                ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
            return profile;
        }

        public async Task<Profile> GetProfileByIdAndCurrentCustomerIdAsync(Guid profileId, Guid customerId)
        {
            var profile = await _repository.FirstOrDefaultAsync(BaseEntityStatus.Active, filters: new()
            {
                p => p.Id == profileId,
                p => p.UserId == customerId
            }, include: i => i.Include(p => p.Wallets!.Where(w => w.Status == BaseEntityStatus.Active)));
            return profile ?? throw new EntityNotFoundException(MessageConstants.ProfileMessageConstrant.ProfileNotFound);
        }
    }
}
