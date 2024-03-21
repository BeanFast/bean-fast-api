﻿using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements;

public class MenuService : BaseService<Menu>, IMenuService
{
    private readonly IKitchenService _kitchenService;
    private readonly IFoodService _foodService;
    private readonly IMenuDetailService _menuDetailService;
    public MenuService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IKitchenService kitchenService, IOptions<AppSettings> appSetting, IFoodService foodService, IMenuDetailService menuDetailService) : base(unitOfWork, mapper, appSetting)
    {
        _kitchenService = kitchenService;
        _foodService = foodService;
        _menuDetailService = menuDetailService;
    }
    private List<Expression<Func<Menu, bool>>> getFilterFromFilterRequest(string userRole, MenuFilterRequest filterRequest)
    {
        List<Expression<Func<Menu, bool>>> filters = new();

        if (filterRequest.KitchenId != Guid.Empty && filterRequest.KitchenId != null)
        {
            filters.Add((f) => f.KitchenId == filterRequest.KitchenId);
        }

        if (filterRequest.Code != null)
        {
            filters.Add(f => f.Code == filterRequest.Code);
        }

        if (filterRequest.CreaterId != Guid.Empty && filterRequest.CreaterId != null)
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
        if (filterRequest.Status != null)
        {
            //if (userRole == null || userRole == RoleName.CUSTOMER.ToString())
            //{
            //    filters.Add(f => f.Status == BaseEntityStatus.Active);
            //}
        }
        if (filterRequest.OrderStartTime != null)
        {
            filters.Add(
                f => f.Sessions!
                .Where(s =>
                    s.OrderStartTime.Date.CompareTo(filterRequest.OrderStartTime.Value.Date) == 0
                    && s.Status == BaseEntityStatus.Active).Any());
        }
        return filters;
    }

    public async Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string userRole, MenuFilterRequest menuFilterRequest)
    {
        IPaginable<GetMenuResponse>? menuPage = null;
        if (userRole == RoleName.ADMIN.ToString())
        {
            menuPage = await _repository.GetPageAsync<GetMenuResponse>(
                paginationRequest: request,
                include: i => i.Include(menu => menu.Kitchen!)
            );
        }
        return menuPage;
    }

    public async Task<ICollection<GetMenuResponse>> GetAllAsync(string userRole, MenuFilterRequest menuFilterRequest)
    {
        var filters = getFilterFromFilterRequest(userRole, menuFilterRequest);
        Func<IQueryable<Menu>, IIncludableQueryable<Menu, object>> include =
            (menu) => menu.Include(menu => menu.Kitchen!)
            .Include(menu => menu.Sessions!)
            .ThenInclude(session => session.SessionDetails!)
            .ThenInclude(sd => sd.Location!);
        return await _repository.GetListAsync<GetMenuResponse>(filters: filters, include: include);

    }
    public async Task CreateMenuAsync(CreateMenuRequest createMenuRequest, Guid createrId)
    {
        await _kitchenService.GetByIdAsync(MenuStatus.Active, createMenuRequest.KitchenId);
        var menuId = Guid.NewGuid();
        var menuEntity = _mapper.Map<Menu>(createMenuRequest);
        menuEntity.CreaterId = createrId;
        menuEntity.UpdaterId = createrId;
        menuEntity.CreateDate = DateTime.UtcNow;
        menuEntity.UpdateDate = DateTime.UtcNow;
        menuEntity.Id = menuId;
        var menuNumber = await _repository.CountAsync() + 1;
        menuEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.MenuCodeConstrant.MenuPrefix, menuNumber);
        menuEntity.Status = BaseEntityStatus.Active;
        var menuDetailNumber = await _menuDetailService.CountAsync();
        foreach (var menuDetail in menuEntity.MenuDetails!)
        {
            
            await _foodService.GetByIdAsync(menuDetail.FoodId);
            menuDetail.Status = BaseEntityStatus.Active;
            menuDetailNumber++;
            menuDetail.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.MenuDetailCodeConstrant.MenuDetailPrefix, menuDetailNumber);
        }
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await _repository.InsertAsync(menuEntity);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await transaction.RollbackAsync();
                throw;
            }
        }
        //menuEntity.

    }

}