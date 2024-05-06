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
    private List<Expression<Func<Menu, bool>>> GetFilterFromFilterRequest(string userRole, MenuFilterRequest filterRequest)
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
            filters.Add(f => f.CreatorId == filterRequest.CreaterId);
        }

        if (filterRequest.CreateDate != null)
        {
            filters.Add(f => f.CreatedDate!.Value.Date == filterRequest.CreateDate.Value.Date);
        }
        if (filterRequest.UpdateDate != null)
        {
            filters.Add(f => f.UpdatedDate!.Value.Date == filterRequest.UpdateDate.Value.Date);
        }
        if (filterRequest.Status != null)
        {

        }
        if (filterRequest.OrderStartTime != null)
        {
            filters.Add(
                f => f.Sessions!.Any(s => s.OrderStartTime.CompareTo(filterRequest.OrderStartTime.Value) == 0
                                          && s.Status == BaseEntityStatus.Active));
        }
        if (RoleName.MANAGER.ToString().Equals(userRole))
        {
            if (filterRequest.SessionExpired)
            {
                filters.Add(
                    m => m.Sessions!.Any(s => s.OrderEndTime < TimeUtil.GetCurrentVietNamTime()));
            }
            if (filterRequest.SessonIncomming)
            {
                filters.Add(
                    m => m.Sessions!.Any(s => s.OrderStartTime > TimeUtil.GetCurrentVietNamTime()));
            }
            if (filterRequest.SessionOrderable)
            {
                filters.Add(
                    m => m.Sessions!.Any(s => s.OrderStartTime <= TimeUtil.GetCurrentVietNamTime() && s.OrderEndTime > TimeUtil.GetCurrentVietNamTime()));
            }
        }
        else
        {
            if (filterRequest.SessionOrderable)
            {
                filters.Add(
                    m => m.Sessions!.Any(s => s.OrderStartTime <= TimeUtil.GetCurrentVietNamTime()
                                              && s.OrderEndTime > TimeUtil.GetCurrentVietNamTime()
                                              && s.Status == BaseEntityStatus.Active));
            }

        }
        if (filterRequest.SchoolId.HasValue && filterRequest.SchoolId != Guid.Empty)
        {
            filters.Add(m => m.Kitchen!.PrimarySchools!.Any(s => s.Id == filterRequest.SchoolId));
        }

        return filters;
    }

    public async Task<Menu> GetByIdAsync(Guid id)
    {
        List<Expression<Func<Menu, bool>>> filters = new()
            {
                (menu) => menu.Id == id
            };
        var menu = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
            filters: filters, include: queryable => queryable
            .Include(m => m.Kitchen!)
            .Include(m => m.MenuDetails!)
            ?? throw new EntityNotFoundException(MessageConstants.MenuMessageConstrant.MenuNotFound(id)));
        return menu;
    }

    public async Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string? userRole, MenuFilterRequest menuFilterRequest)
    {
        IPaginable<GetMenuResponse>? menuPage = null;
        if (userRole == RoleName.ADMIN.ToString())
        {
            menuPage = await _repository.GetPageAsync<GetMenuResponse>(
                paginationRequest: request,
                include: i => i.Include(menu => menu.Kitchen!).Include(m => m.Sessions!)
                    .ThenInclude(s => s.SessionDetails!)
                    .ThenInclude(sd => sd.Location!)
            );
        }
        return menuPage!;
    }

    public async Task<ICollection<GetMenuResponse>> GetAllAsync(string? userRole, MenuFilterRequest menuFilterRequest)
    {
        var filters = GetFilterFromFilterRequest(userRole, menuFilterRequest);
        Func<IQueryable<Menu>, IIncludableQueryable<Menu, object>> include =
            (menu) => menu.Include(menu => menu.Kitchen!)
            .Include(menu => menu.Sessions!)
            .ThenInclude(session => session.SessionDetails!)
            .ThenInclude(sd => sd.Location!);
        return await _repository.GetListAsync<GetMenuResponse>(filters: filters, include: include);

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

    public async Task<GetMenuResponse> GetGetMenuResponseByIdAsync(Guid id)
    {
        List<Expression<Func<Menu, bool>>> filters = new()
            {
                (menu) => menu.Id == id
            };
        var menu = await _repository.FirstOrDefaultAsync<GetMenuResponse>(status: BaseEntityStatus.Active,
            filters: filters, include: queryable => queryable
            .Include(m => m.Kitchen!)
            .Include(m => m.MenuDetails!).ThenInclude(md => md.Food!))
            ?? throw new EntityNotFoundException(MessageConstants.MenuMessageConstrant.MenuNotFound(id));
        return menu;
    }

    public async Task UpdateMenuAsync(UpdateMenuRequest request, Guid guid, User updater)
    {
        await _kitchenService.GetByIdAsync(BaseEntityStatus.Active, request.KitchenId);
        var menuEntity = await GetByIdAsync(guid);
        var menuDetails = new List<MenuDetail>();
        var menuDetailNumber = await _menuDetailService.CountAsync();
        menuEntity.UpdateDate = TimeUtil.GetCurrentVietNamTime();
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
            }) ;
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