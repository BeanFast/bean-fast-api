using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.CardType.Request;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class CardTypeService : BaseService<CardType>, ICardTypeService
    {
        private readonly ICloudStorageService _cloudStorageService;
        public CardTypeService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
        }

        public async Task CreateCardTypeAsync(CreateCardTypeRequest request)
        {
            var cardTypeEntity = _mapper.Map<CardType>(request);
            var cardId = Guid.NewGuid();
            var imagePath = await _cloudStorageService.UploadFileAsync(cardId, _appSettings.Firebase.FolderNames.CardType, request.Image);
            cardTypeEntity.Id = cardId;
            cardTypeEntity.Status = BaseEntityStatus.Active;
            cardTypeEntity.BackgroundImagePath = imagePath;
            var cardNumber = await _repository.CountAsync() + 1;    
            cardTypeEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.CardTypeCodeConstrant.CardTypePrefix, cardNumber);
            await _repository.InsertAsync(cardTypeEntity);
            await _unitOfWork.CommitAsync();
            //return ;
        }

        public async Task<ICollection<GetCardTypeResponse>> GetAllAsync()
        {
            var cardTypes = await _repository.GetListAsync<GetCardTypeResponse>(
                BaseEntityStatus.Active);
            return cardTypes;
        }

        public async Task<CardType> GetByIdAsync(Guid id)
        {
            return await _repository
                .FirstOrDefaultAsync(BaseEntityStatus.Active, filters: new() { c => c.Id == id }) ??
                throw new EntityNotFoundException(MessageConstants.CardTypeMessageConstrant.CardTypeNotFound(id));
        }

        public async Task UpdateCardTypeAsync(Guid id, UpdateCardTypeRequest request)
        {
            var cardType = await GetByIdAsync(id);
            cardType.Name = request.Name;
            cardType.Width = request.Width;
            cardType.Height = request.Height;
            if (request.Image is not null)
            {
                await _cloudStorageService.DeleteFileAsync(id, _appSettings.Firebase.FolderNames.CardType);
                cardType.BackgroundImagePath = await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.CardType, request.Image);
            }
            await _repository.UpdateAsync(cardType);
            await _unitOfWork.CommitAsync();
        }
        public async Task DeleteCardTypeAsync(Guid id)
        {
            var cardType = await GetByIdAsync(id);
            await _repository.DeleteAsync(cardType);
            await _unitOfWork.CommitAsync();
        }

    }
}
