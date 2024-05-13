using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class KitchenRepository : GenericRepository<Kitchen>, IKitchenRepository
    {
        public KitchenRepository(BeanFastContext context, IMapper mapper) : base(context, mapper) 
        {

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
                page = await GetPageAsync<GetKitchenResponse>(
                    paginationRequest: paginationRequest, filters: filters);
            }
            else
            {
                filters.Add(k => k.Status != BaseEntityStatus.Deleted);
                page = await GetPageAsync<GetKitchenResponse>(
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
            return await FirstOrDefaultAsync(filters: new()
        {
            kitchen => kitchen.Id == id
        }) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
        }

        public async Task<Kitchen> GetByIdAsync(int status, Guid id)
        {
            return await FirstOrDefaultAsync(filters: new()
        {
            kitchen => kitchen.Id == id,
            kitchen => kitchen.Status == status
        }) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
        }

        public async Task<Kitchen> GetByIdIncludePrimarySchoolsAsync(Guid id)
        {
            return await FirstOrDefaultAsync(filters: new()
        {
            kitchen => kitchen.Id == id && kitchen.Status != BaseEntityStatus.Deleted
        }, include: i => i.Include(k => k.PrimarySchools!)) ?? throw new EntityNotFoundException(MessageConstants.KitchenMessageConstrant.KitchenNotFound(id));
        }

        public async Task<int> CountSchoolByKitchenIdAsync(Guid kitchentId)
        {
            var kitchenEntity = await GetByIdIncludePrimarySchoolsAsync(kitchentId);

            return kitchenEntity.PrimarySchools!.Count;
        }
        public async Task<ICollection<GetKitchenResponse>> GetAllAsync(string? userRole, KitchenFilterRequest filterRequest)
        {
            var filters = GetKitchenFilterFromFilterRequest(filterRequest);
            ICollection<GetKitchenResponse> kitchens = default!;
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                kitchens = await GetListAsync<GetKitchenResponse>(
                    filters: filters);
            }
            else
            {
                filters.Add(k => k.Status != BaseEntityStatus.Deleted);
                kitchens = await GetListAsync<GetKitchenResponse>(
                    filters: filters);
            }
            foreach (var item in kitchens)
            {
                item.SchoolCount = await CountSchoolByKitchenIdAsync(item.Id);
            }
            return kitchens;
        }

    }

}
