using AutoMapper;
using Azure.Core;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements;

public class KitchenService : BaseService<Kitchen>, IKitchenService
{
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IAreaService _areaService;
    private readonly IKitchenRepository _repository;
    public KitchenService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService, IKitchenRepository repository) : base(unitOfWork, mapper, appSettings)
    {
        _cloudStorageService = cloudStorageService;
        _areaService = areaService;
        _repository = repository;
    }
    

    public async Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole)

    {
       return await _repository.GetKitchenPageAsync(paginationRequest, filterRequest, userRole);
    }

    public async Task<Kitchen> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Kitchen> GetByIdAsync(int status, Guid id)
    {
        return await _repository.GetByIdAsync(status, id);
    }

    public async Task CreateKitchenAsync(CreateKitchenRequest request, User user)
    {
        var kitchenEntity = _mapper.Map<Kitchen>(request);
        var kitchenId = Guid.NewGuid();
        string imageUrl = await _cloudStorageService.UploadFileAsync(kitchenId, _appSettings.Firebase.FolderNames.Kitchen, request.Image);
        kitchenEntity.ImagePath = imageUrl;
        kitchenEntity.Status = BaseEntityStatus.Active;
        kitchenEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.KitchenCodeConstrant.KitchenPrefix, await _repository.CountAsync());
        await _areaService.GetAreaByIdAsync(request.AreaId);
        //kitchenEntity.Area = areaEntity;
        await _repository.InsertAsync(kitchenEntity, user);
        await _unitOfWork.CommitAsync();

    }
    public async Task DeleteKitchenAsync(Guid id, User user)
    {
        var kitchenEntity = await GetByIdAsync(BaseEntityStatus.Active, id);
        await _repository.DeleteAsync(kitchenEntity, user);
        await _unitOfWork.CommitAsync();
    }
    public async Task<Kitchen> GetByIdIncludePrimarySchoolsAsync(Guid id)
    {
        return await _repository.GetByIdIncludePrimarySchoolsAsync(id);
    }

    public async Task<int> CountSchoolByKitchenIdAsync(Guid kitchentId)
    {
        return await _repository.CountSchoolByKitchenIdAsync(kitchentId);
    }

    public async Task UpdateKitchenAsync(Guid id, UpdateKitchentRequest request, User user)
    {
        var kitchen = await GetByIdAsync(id);
        if (request.Image != null)
        {
            string imageUrl = await _cloudStorageService.UploadFileAsync(kitchen.Id, _appSettings.Firebase.FolderNames.Kitchen, request.Image);
            kitchen.ImagePath = imageUrl;
        }
        await _areaService.GetAreaByIdAsync(request.AreaId);
        kitchen.AreaId = request.AreaId;
        kitchen.Address = request.Address;
        kitchen.Name = request.Name;
        //kitchenEntity.Area = areaEntity;
        await _repository.UpdateAsync(kitchen, user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<ICollection<GetKitchenResponse>> GetAllAsync(string? userRole, KitchenFilterRequest filterRequest)
    {
        return await _repository.GetAllAsync(userRole, filterRequest);
    }
}