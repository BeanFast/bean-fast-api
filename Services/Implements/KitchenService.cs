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

namespace Services.Implements;

public class KitchenService : BaseService<Kitchen>, IKitchenService
{
    private readonly ICloudStorageService _cloudStorageService;
    private readonly IAreaService _areaService;
    public KitchenService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService) : base(unitOfWork, mapper, appSettings)
    {
        _cloudStorageService = cloudStorageService;
        _areaService = areaService;
    }
    private List<Expression<Func<Kitchen, bool>>> GetKitchenFilterFromFilterRequest(KitchenFilterRequest filterRequest)
    {
        List<Expression<Func<Kitchen, bool>>> filters = new();
        if (filterRequest.Code != null)
        {
            filters.Add(p => p.Code == filterRequest.Code);
        }
        if (filterRequest.Name != null)
        {
            filters.Add(k => k.Name.ToLower() == filterRequest.Name.ToLower());
        }
        if (filterRequest.AreaId != Guid.Empty && filterRequest.AreaId != null)
        {
            filters.Add(k => k.AreaId == filterRequest.AreaId);
        }
        return filters;
    }

    public async Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole)

    {
        var filters = GetKitchenFilterFromFilterRequest(filterRequest);
        IPaginable<GetKitchenResponse> page = default!;
        if (RoleName.ADMIN.ToString().Equals(userRole))
        {
            page = await _repository.GetPageAsync<GetKitchenResponse>(
                paginationRequest: paginationRequest, filters: filters);
        }
        else
        {
            page = await _repository.GetPageAsync<GetKitchenResponse>(
                status: BaseEntityStatus.Active,
                paginationRequest: paginationRequest, filters: filters);
        }
        foreach (var item in page.Items)
        {
            item.SchoolCount = await CountSchoolByKitchenIdAsync(item.Id);
        }
        return page;
    }

    public async Task<Kitchen> GetByIdAsync(Guid id)
    {
        return await _repository.FirstOrDefaultAsync(filters: new()
        {
            kitchen => kitchen.Id == id
        }) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
    }

    public async Task<Kitchen> GetByIdAsync(int status, Guid id)
    {
        return await _repository.FirstOrDefaultAsync(filters: new()
        {
            kitchen => kitchen.Id == id,
            kitchen => kitchen.Status == status
        }) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
    }

    public async Task CreateKitchenAsync(CreateKitchenRequest request)
    {
        var kitchenEntity = _mapper.Map<Kitchen>(request);
        var kitchenId = Guid.NewGuid();
        string imageUrl = await _cloudStorageService.UploadFileAsync(kitchenId, _appSettings.Firebase.FolderNames.Kitchen, request.Image);
        kitchenEntity.ImagePath = imageUrl;
        kitchenEntity.Status = BaseEntityStatus.Active;
        await _areaService.GetAreaByIdAsync(request.AreaId);
        //kitchenEntity.Area = areaEntity;
        await _repository.InsertAsync(kitchenEntity);
        await _unitOfWork.CommitAsync();

    }
    public async Task DeleteKitchenAsync(Guid id)
    {
        var kitchenEntity = await GetByIdAsync(BaseEntityStatus.Active, id);
        await _repository.DeleteAsync(kitchenEntity);
        await _unitOfWork.CommitAsync();
    }
    public async Task<Kitchen> GetByIdIncludePrimarySchoolsAsync(Guid id)
    {
        return await _repository.FirstOrDefaultAsync(BaseEntityStatus.Active, filters: new()
        {
            kitchen => kitchen.Id == id
        }, include: i => i.Include(k => k.PrimarySchools!)) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
    }

    public async Task<int> CountSchoolByKitchenIdAsync(Guid kitchentId)
    {
        var kitchenEntity = await GetByIdIncludePrimarySchoolsAsync(kitchentId);

        return kitchenEntity.PrimarySchools!.Count;
    }

    public async Task UpdateKitchenAsync(Guid id, UpdateKitchentRequest request)
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
        await _repository.UpdateAsync(kitchen);
        await _unitOfWork.CommitAsync();
    }
}