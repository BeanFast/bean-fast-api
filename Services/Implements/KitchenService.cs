using AutoMapper;
using Azure.Core;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
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
    private readonly AppSettings _appSettings;
    private readonly IAreaService _areaService;
    public KitchenService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, IAreaService areaService) : base(unitOfWork, mapper)
    {
        _cloudStorageService = cloudStorageService;
        _appSettings = appSettings.Value;
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
        if (filterRequest.AreaId != Guid.Empty)
        {
            filters.Add(k => k.AreaId == filterRequest.AreaId);
        }
        return filters;
    }

    public async Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole)

    {
        var filters = GetKitchenFilterFromFilterRequest(filterRequest);
        Expression<Func<Kitchen, GetKitchenResponse>> selector = (k => _mapper.Map<GetKitchenResponse>(k));
        IPaginable<GetKitchenResponse> page = default!;
        if (RoleName.ADMIN.ToString().Equals(userRole))
        {
            page = await _repository.GetPageAsync(
                paginationRequest: paginationRequest, 
                filters: filters, selector: selector);
        }
        else
        {
            page = await _repository.GetPageAsync(
                status: BaseEntityStatus.Active, paginationRequest: paginationRequest, 
                filters: filters, selector: selector);
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

    }
    public async Task DeleteKitchenAsync(Guid id)
    {
        var kitchenEntity = await GetByIdAsync(BaseEntityStatus.Active, id);
        await _repository.DeleteAsync(kitchenEntity);
    }
}