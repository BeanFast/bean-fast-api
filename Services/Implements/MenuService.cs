using AutoMapper;
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
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements;

public class MenuService : BaseService<Menu>, IMenuService
{
    private readonly IKitchenService _kitchenService;
    private readonly IFoodService _foodService;
    private readonly IMenuDetailService _menuDetailService;
    private readonly IMenuRepository _repository;
    public MenuService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IKitchenService kitchenService, IOptions<AppSettings> appSetting, IFoodService foodService, IMenuDetailService menuDetailService, IMenuRepository repository) : base(unitOfWork, mapper, appSetting)
    {
        _kitchenService = kitchenService;
        _foodService = foodService;
        _menuDetailService = menuDetailService;
        _repository = repository;
    }


    public async Task<Menu> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string? userRole, MenuFilterRequest menuFilterRequest)
    {
        return await _repository.GetPageAsync(request, userRole, menuFilterRequest);
    }

    public async Task<ICollection<GetMenuResponse>> GetAllAsync(string? userRole, MenuFilterRequest menuFilterRequest)
    {
        return await _repository.GetAllAsync(userRole, menuFilterRequest);

    }
    public async Task<GetMenuResponse> GetGetMenuResponseByIdAsync(Guid id)
    {
        return await _repository.GetGetMenuResponseByIdAsync(id);
    }


    public async Task CreateMenuAsync(CreateMenuRequest createMenuRequest, User creator)
    {
        await _kitchenService.GetByIdAsync(BaseEntityStatus.Active, createMenuRequest.KitchenId);
        var menuId = Guid.NewGuid();
        var menuEntity = _mapper.Map<Menu>(createMenuRequest);

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
                await _repository.InsertAsync(menuEntity, creator);
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

    public async Task DeleteMenuAsync(Guid id)
    {
        var currentVietnamTime = TimeUtil.GetCurrentVietNamTime();
        List<Expression<Func<Menu, bool>>> filters = new()
        {
            m => m.Id == id,
            m => m.Status == BaseEntityStatus.Active
        };

        var menu = await _repository.FirstOrDefaultAsync(
            filters,
            include: i => i.Include(m => m.Sessions!.Where(
                s => currentVietnamTime >= s.OrderStartTime && currentVietnamTime <= s.OrderEndTime
                && s.Status == BaseEntityStatus.Active
                ))
            ) ?? throw new InvalidRequestException(MessageConstants.MenuMessageConstrant.MenuNotFound(id));
        if (menu.Sessions!.Any())
        {
            throw new InvalidRequestException(MessageConstants.MenuMessageConstrant.MenuAlreadyOnSell);
        }
        await _repository.DeleteAsync(menu);
        await _unitOfWork.CommitAsync();
    }
    

    public async Task UpdateMenuAsync(UpdateMenuRequest request, Guid guid, User updater)
    {
        await _kitchenService.GetByIdAsync(BaseEntityStatus.Active, request.KitchenId);
        var menuEntity = await GetByIdAsync(guid);
        var menuDetails = new List<MenuDetail>();
        var menuDetailNumber = await _menuDetailService.CountAsync();
        menuEntity.UpdatedDate = TimeUtil.GetCurrentVietNamTime();
        menuEntity.Updater = updater;
        foreach (var item in request.MenuDetails)
        {
            await _foodService.GetByIdAsync(item.FoodId);
            menuDetails.Add(new MenuDetail()
            {
                Id = Guid.NewGuid(),
                MenuId = guid,
                Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.MenuDetailCodeConstrant.MenuDetailPrefix, menuDetailNumber++),
                Status = BaseEntityStatus.Active,
                FoodId = item.FoodId,
                Price = item.Price,
            });
        }
        menuEntity.MenuDetails = new List<MenuDetail>();
        using (var transaction = await _unitOfWork.BeginTransactionAsync())
        {
            try
            {
                await _menuDetailService.InsertRangeAsync(menuDetails);
                await _repository.UpdateAsync(menuEntity);
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
    }
}