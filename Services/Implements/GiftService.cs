using AutoMapper;
using Azure.Core;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Gift.Request;
using DataTransferObjects.Models.Gift.Response;
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
    public class GiftService : BaseService<Gift>, IGiftService
    {

        private readonly ICloudStorageService _cloudStorageService;
        private readonly IGiftRepository _repository;

        public GiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService, IGiftRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
            _repository = repository;
        }

        public async Task CreateGiftAsync(CreateGiftRequest request, User user)
        {
            var giftEntity = _mapper.Map<Gift>(request);
            var giftId = Guid.NewGuid();
            var giftNumber = await _repository.CountAsync() + 1;
            giftEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.GiftCodeConstrant.GiftPrefix, giftNumber);
            giftEntity.Status = BaseEntityStatus.Active;
            giftEntity.Id = giftId;

            var imagePath = await _cloudStorageService.UploadFileAsync(giftId, _appSettings.Firebase.FolderNames.Gift, request.Image);
            giftEntity.ImagePath = imagePath;
            await _repository.InsertAsync(giftEntity, user);
            await _unitOfWork.CommitAsync();
        }
        
        public async Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest)
        {
            return await _repository.GetGiftPageAsync(paginationRequest, filterRequest);
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id, int status)
        {
            return await _repository.GetGiftByIdAsync(id, status);
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id)
        {
            return await _repository.GetGiftByIdAsync(id);
        }

        public async Task UpdateGiftAsync(Guid id, UpdateGiftRequest request, User user)
        {
            var gift = await GetGiftByIdAsync(id, BaseEntityStatus.Active);
            gift.Name = request.Name;
            gift.Points = request.Points;
            gift.InStock = request.InStock;
            if (request.Image != null)
            {
                await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.Gift, request.Image);
            }
            await _repository.UpdateAsync(gift, user);
            await _unitOfWork.CommitAsync();
        }
        public async Task UpdateGiftAsync(Gift gift)
        {
            await _repository.UpdateAsync(gift);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteGiftAsync(Guid id, User user)
        {
            var gift = await GetGiftByIdAsync(id, BaseEntityStatus.Active);
            await _repository.DeleteAsync(gift, user);
            await _unitOfWork.CommitAsync();
        }


    }
}
