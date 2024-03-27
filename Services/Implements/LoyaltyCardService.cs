using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.LoyaltyCard.Request;
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
using Utilities.Utils;

namespace Services.Implements
{
    public class LoyaltyCardService : BaseService<LoyaltyCard>, ILoyaltyCardService
    {
        private readonly IProfileService _profileService;
        private readonly ICardTypeService _cardTypeService;
        private readonly ICloudStorageService _cloudStorageService;
        public LoyaltyCardService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IProfileService profileService, ICardTypeService cardTypeService, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper, appSettings)
        {
            _profileService = profileService;
            _cardTypeService = cardTypeService;
            _cloudStorageService = cloudStorageService;
        }
        public async Task<bool> CheckLoyaltyCardWithQRCode(string qrCode)
        {
            if(string.IsNullOrEmpty(qrCode))
            {
                throw new InvalidRequestException(MessageConstants.LoyaltyCardMessageConstrant.QRCodeNotFound);
            }

            List<Expression<Func<LoyaltyCard, bool>>> filters = new()
            {
                (loyaltyCard) => qrCode.Equals(loyaltyCard.QRCode)
            };
            var loyaltyCard = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters);

            if (loyaltyCard != null)
            {
                return true;
            }

            return false;
        }

        public async Task CreateLoyaltyCard(CreateLoyaltyCardRequest request)
        {
            var loyaltyCard = _mapper.Map<LoyaltyCard>(request);
            await _profileService.GetByIdAsync(loyaltyCard.Id);
            await _cardTypeService.GetByIdAsync(loyaltyCard.CardTypeId);
            loyaltyCard.QRCode = Guid.NewGuid().ToString();
            loyaltyCard.Status = BaseEntityStatus.Active;
            loyaltyCard.Id = Guid.NewGuid();
            loyaltyCard.BackgroundImagePath = await _cloudStorageService.UploadFileAsync(loyaltyCard.Id, _appSettings.Firebase.FolderNames.LoyaltyCard, request.Image);
            var entityNumber = await _repository.CountAsync();
            loyaltyCard.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.LoyaltyCardCodeConstraint.LoyaltyCardPrefix, entityNumber);
           
            await _repository.InsertAsync(loyaltyCard);
            await _unitOfWork.CommitAsync();
        }

        public Task<LoyaltyCard> GetLoyaltyCardByQRCode(string qrCode)
        {
            List<Expression<Func<LoyaltyCard, bool>>> filters = new()
            {
                (loyaltyCard) => qrCode.Equals(loyaltyCard.QRCode)
            };
            var loyaltyCard = _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters,
                include: queryable => queryable.Include(lc => lc.Profile!))
                ?? throw new EntityNotFoundException(MessageConstants.LoyaltyCardMessageConstrant.InvalidLoyaltyCard);
            return loyaltyCard!;
        }
    }
}
