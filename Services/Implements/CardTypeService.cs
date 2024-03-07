using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.CardType.Request;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;
using Utilities.Statuses;

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
            //var imagePath = await _cloudStorageService.UploadFileAsync(cardId, _appSettings.Firebase.FolderNames.CardType, request.Image);
            cardTypeEntity.Id = cardId;
            await _repository.InsertAsync(cardTypeEntity);
            //return ;
        }

        public async Task<ICollection<GetCardTypeResponse>> GetAllAsync()
        {
            var cardTypes = await _repository.GetListAsync(
                BaseEntityStatus.Active,
                selector: c => _mapper.Map<GetCardTypeResponse>(c)
                );
            return cardTypes;
        }

        public async Task UpdateCardTypeAsync(UpdateCardTypeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
