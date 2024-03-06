using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Enums;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements;

public class MenuService : BaseService<Menu>, IMenuService
{
    private readonly IKitchenService _kitchenService;
    public MenuService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IKitchenService kitchenService, IOptions<AppSettings> appSetting) : base(unitOfWork, mapper, appSetting)
    {
        _kitchenService = kitchenService;
    }
    private List<Expression<Func<Menu, bool>>> GetFilterFromFilterRequest(string userRole, MenuFilterRequest filterRequest)
    {
        List<Expression<Func<Menu, bool>>> filters = new();

        if (filterRequest.KitchenId != Guid.Empty)
        {
            filters.Add((f) => f.KitchenId == filterRequest.KitchenId);
        }

        if (filterRequest.Code != null)
        {
            filters.Add(f => f.Code == filterRequest.Code);
        }

        if (filterRequest.CreaterId != Guid.Empty)
        {
            filters.Add(f => f.CreaterId == filterRequest.CreaterId);
        }

        if (filterRequest.CreateDate != null)
        {
            filters.Add(f => f.CreateDate!.Value.Date == filterRequest.CreateDate.Value.Date);
        }
        if (filterRequest.UpdateDate != null)
        {
            filters.Add(f => f.UpdateDate!.Value.Date == filterRequest.UpdateDate.Value.Date);
        }
        if(filterRequest.Status != null)
        {
            if(userRole == null || userRole == RoleName.CUSTOMER.ToString())
            {
                filters.Add(f => f.Status == BaseEntityStatus.Active);
            }
        }
        return filters;
    }

    public async Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string userRole, MenuFilterRequest menuFilterRequest)
    {
        IPaginable<GetMenuResponse>? menuPage = null;
        if (userRole == RoleName.ADMIN.ToString())
        {
            menuPage = await _repository.GetPageAsync(
                paginationRequest: request,
                include: i => i.Include(menu => menu.Kitchen),
                selector: menu => _mapper.Map<GetMenuResponse>(menu)
            );
        }
        return menuPage;
    }

    public async Task<ICollection<GetMenuResponse>> GetAllAsync(string userRole, MenuFilterRequest menuFilterRequest)
    {
        var filters = GetFilterFromFilterRequest(userRole, menuFilterRequest);
        Expression<Func<Menu, GetMenuResponse>> selector = (menu) => _mapper.Map<GetMenuResponse>(menu);
        Func<IQueryable<Menu>, IIncludableQueryable<Menu, object>> include = (menu) => menu.Include(menu => menu.Kitchen);
        return await _repository.GetListAsync(selector: selector, filters: filters, include: include);

    }
    public async Task CreateMenuAsync(CreateMenuRequest createMenuRequest, Guid createrId)
    {
        await _kitchenService.GetByIdAsync(MenuStatus.Active, createMenuRequest.KitchenId);

        var menuEntity = _mapper.Map<Menu>(createMenuRequest);
        menuEntity.CreaterId = createrId;
        menuEntity.UpdaterId = createrId;
        menuEntity.CreateDate = DateTime.UtcNow;
        menuEntity.UpdateDate = DateTime.UtcNow;
        //menuEntity.

    }

}