using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;
using Utilities.Utils;

namespace Repositories.Implements;

public class MenuRepository : GenericRepository<Menu>, IMenuRepository
{
    public MenuRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {

    }
    private List<Expression<Func<Menu, bool>>> GetFilterFromFilterRequest(string? userRole, MenuFilterRequest filterRequest)
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
        if (RoleName.ADMIN.ToString().Equals(userRole))
        {

        }
        else
        {
            filters.Add(m => m.Status != BaseEntityStatus.Deleted);
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
                (menu) => menu.Id == id && menu.Status != BaseEntityStatus.Deleted
            };
        var menu = await FirstOrDefaultAsync(
            filters: filters, include: queryable => queryable
            .Include(m => m.Kitchen!)
            .Include(m => m.MenuDetails!))
            ?? throw new EntityNotFoundException(MessageConstants.MenuMessageConstrant.MenuNotFound(id));
        return menu;
    }

    public async Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string? userRole, MenuFilterRequest menuFilterRequest)
    {
        IPaginable<GetMenuResponse>? menuPage = null;
        if (userRole == RoleName.ADMIN.ToString())
        {
            menuPage = await GetPageAsync<GetMenuResponse>(
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
        return await GetListAsync<GetMenuResponse>(filters: filters, include: include);

    }
    public async Task<ICollection<GetMenuResponse>> GetAllAsync(User manager, MenuFilterRequest menuFilterRequest)
    {
        var filters = GetFilterFromFilterRequest(manager!.Role!.EnglishName, menuFilterRequest);
        filters.Add(m => m.KitchenId == manager.Kitchen!.Id);
        Func<IQueryable<Menu>, IIncludableQueryable<Menu, object>> include =
            (menu) => menu.Include(menu => menu.Kitchen!)
            .Include(menu => menu.Sessions!)
            .ThenInclude(session => session.SessionDetails!)
            .ThenInclude(sd => sd.Location!);
        return await GetListAsync<GetMenuResponse>(filters: filters, include: include);
    }

    public async Task<GetMenuResponse> GetGetMenuResponseByIdAsync(Guid id)
    {
        List<Expression<Func<Menu, bool>>> filters = new()
            {
                (menu) => menu.Id == id && menu.Status != BaseEntityStatus.Deleted
            };
        var menu = await FirstOrDefaultAsync<GetMenuResponse>(
            filters: filters, include: queryable => queryable
            .Include(m => m.Kitchen!)
            .Include(m => m.MenuDetails!).ThenInclude(md => md.Food!))
            ?? throw new EntityNotFoundException(MessageConstants.MenuMessageConstrant.MenuNotFound(id));
        return menu;
    }


}