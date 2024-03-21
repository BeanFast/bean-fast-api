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
        public GiftService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper, appSettings)
        {
            _cloudStorageService = cloudStorageService;
        }

        public async Task CreateGiftAsync(CreateGiftRequest request)
        {
            var giftEntity = _mapper.Map<Gift>(request);
            var giftId = Guid.NewGuid();
            var giftNumber = await _repository.CountAsync() + 1;
            giftEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.GiftCodeConstrant.GiftPrefix, giftNumber);
            giftEntity.Status = BaseEntityStatus.Active;
            giftEntity.Id = giftId;

            var imagePath = await _cloudStorageService.UploadFileAsync(giftId, _appSettings.Firebase.FolderNames.Gift, request.Image);
            giftEntity.ImagePath = imagePath;
            await _repository.InsertAsync(giftEntity);
            await _unitOfWork.CommitAsync();
        }
        private List<Expression<Func<Gift, bool>>> getFiltersFromFGiftFilterRequest(GiftFilterRequest filterRequest)
        {
            List<Expression<Func<Gift, bool>>> filters = new();

            if (filterRequest.Code != null)
            {
                filters.Add(f => f.Code == filterRequest.Code);
            }

            if (filterRequest.Name is { Length: > 0 })
            {
                filters.Add(f => f.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }

            //if (filterRequest.Points > 0)
            //{
            //    filters.Add(f => f.Points >= filterRequest.Points);
            //}

            return filters;
        }
        public async Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest)
        {
            var filters = getFiltersFromFGiftFilterRequest(filterRequest);
            var page = await _repository.GetPageAsync<GetGiftResponse>(
                    status: BaseEntityStatus.Active, 
                    paginationRequest: paginationRequest,filters: filters);
            return page;
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id, int status)
        {
            List<Expression<Func<Gift, bool>>> filters = new()
            {
                f => f.Id == id,
            };
            var gift =  await _repository.FirstOrDefaultAsync(status: status, filters: filters);
            if(gift == null)
            {
                throw new EntityNotFoundException(MessageConstants.GiftMessageConstrant.GiftNotFound(id));
            }
            return gift;    
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id)
        {
            List<Expression<Func<Gift, bool>>> filters = new()
            {
                f => f.Id == id,
            };
            var gift = await _repository.FirstOrDefaultAsync(filters: filters);
            if (gift == null)
            {
                throw new EntityNotFoundException(MessageConstants.GiftMessageConstrant.GiftNotFound(id));
            }
            return gift;
        }
        public async Task<Gift> GetGiftByIdAsync(int status, Guid id)
        {
            List<Expression<Func<Gift, bool>>> filters = new()
            {
                f => f.Id == id,
            };
            var gift = await _repository.FirstOrDefaultAsync(status: status, filters: filters);
            if (gift == null)
            {
                throw new EntityNotFoundException(MessageConstants.GiftMessageConstrant.GiftNotFound(id));
            }
            return gift;
        }

        public async Task UpdateGiftAsync(Guid id, UpdateGiftRequest request)
        {
            var gift = await GetGiftByIdAsync(id, BaseEntityStatus.Active);
            gift.Name = request.Name;
            gift.Points = request.Points;
            gift.InStock = request.InStock;
            if(request.Image != null)
            {
                await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.Gift, request.Image);
            }
            await _repository.UpdateAsync(gift);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteGiftAsync(Guid id)
        {
            var gift = await GetGiftByIdAsync(id, BaseEntityStatus.Active);
            await _repository.DeleteAsync(gift);
            await _unitOfWork.CommitAsync();
        }


        
    }
}
