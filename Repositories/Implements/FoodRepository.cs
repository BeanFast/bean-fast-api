using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.Statuses;
using DataTransferObjects.Core.Pagination;
using Utilities.Constants;
using Utilities.Exceptions;
using Azure.Core;

namespace Repositories.Implements
{
    public class FoodRepository : GenericRepository<Food>, IFoodRepository
    {
        public FoodRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public List<Expression<Func<Food, bool>>> GetFilterFromFilterRequest(FoodFilterRequest filterRequest)
        {
            List<Expression<Func<Food, bool>>> filters = new();

            if (filterRequest.CategoryId != null && filterRequest.CategoryId != Guid.Empty)
            {
                filters.Add((f) => f.CategoryId == filterRequest.CategoryId);
            }

            if (filterRequest.Code != null)
            {
                filters.Add(f => f.Code == filterRequest.Code);
            }

            if (filterRequest.Name is { Length: > 0 })
            {
                filters.Add(f => f.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }

            if (filterRequest.MinPrice > 0)
            {
                filters.Add(f => f.Price >= filterRequest.MinPrice);
            }
            if (filterRequest.MaxPrice > 0)
            {
                filters.Add(f => f.Price <= filterRequest.MaxPrice);
            }
            if (filterRequest.IsCombo != null)
            {
                filters.Add(f => f.IsCombo == filterRequest.IsCombo);
            }
            if (filterRequest.CreateStartDate != null)
            {
                filters.Add(f => f.CreatedDate!.Value.Date >= filterRequest.CreateStartDate.Value.Date);
            }
            if (filterRequest.CreateEndDate != null)
            {
                filters.Add(f => f.CreatedDate!.Value.Date <= filterRequest.CreateEndDate!.Value.Date);
            }

            return filters;
        }
        public async Task<ICollection<GetFoodResponse>> GetAllFoodsAsync(string? userRole, FoodFilterRequest filterRequest)
        {

            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include = (f) => f.Include(f => f.Category!);
            var filters = GetFilterFromFilterRequest(filterRequest);
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await GetListAsync<GetFoodResponse>(include: include, filters: filters);
            }
            filters.Add(f => f.Status != BaseEntityStatus.Deleted);
            return await GetListAsync<GetFoodResponse>(include: include, filters: filters);
        }
        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest,
            PaginationRequest request)
        {
            IPaginable<GetFoodResponse>? page = null;
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                page = await GetPageAsync<GetFoodResponse>(paginationRequest: request, orderBy: orderBy);
            }
            else
            {
                var filters = new List<Expression<Func<Food, bool>>>();
                filters.Add(f => f.Status != BaseEntityStatus.Deleted);
                page = await GetPageAsync<GetFoodResponse>(filters: filters,
                    paginationRequest: request, orderBy: orderBy);
            }

            return page;
        }
        public async Task<Food?> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Food, bool>>> filters = new()
            {
                (food) => food.Id == id && food.Status != BaseEntityStatus.Deleted,
            };

            var food = await FirstOrDefaultAsync(
                filters: filters, include: queryable => queryable.Include(f => f.Category!).Include(f => f.Combos!).Include(f => f.MasterCombos!));
            return food;
        }
        public async Task<IPaginable<Food>> GetBestSellerFoodsPageAsync(GetBestSellerFoodsRequest request, User manager)
        {
            var filters = new List<Expression<Func<Food, bool>>>();
            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include;
            filters.Add(f => f.OrderDetails!.Any(od => od.Order!.Status == OrderStatus.Completed) && f.Status == BaseEntityStatus.Active);
            if (manager.Kitchen != null)
            {
                filters.Add(f => f.MenuDetails!.Any(md => md.Menu!.KitchenId == manager.Kitchen!.Id));
            }
            if (request.StartDate != DateTime.MinValue && request.EndDate != DateTime.MinValue)
            {
                include = i => i
                .Include(f => f.OrderDetails!
                    .Where(od => od.Order!.PaymentDate.Date >= request.StartDate.Date && od.Order.PaymentDate.Date <= request.EndDate.Date && od.Order.Status == OrderStatus.Completed)
                );
            }
            else
            {
                include = i => i.Include(f => f.OrderDetails!.Where(od => od.Order!.Status == OrderStatus.Completed));
            }
            return await GetPageAsync(
                filters: filters,
                include: include,
                orderBy: o => o.OrderByDescending(f => f.OrderDetails!.Count),
                paginationRequest: new PaginationRequest
                {
                    Page = 1,
                    Size = request.Number
                });
        }
    }
}
