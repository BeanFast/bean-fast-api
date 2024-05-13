using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Gift.Request;
using DataTransferObjects.Models.Gift.Response;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class GiftRepository : GenericRepository<Gift>, IGiftRepository
    {
        public GiftRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id, int status)
        {
            List<Expression<Func<Gift, bool>>> filters = new()
            {
                f => f.Id == id && f.Status == status,
            };
            var gift = await FirstOrDefaultAsync(filters: filters);
            if (gift == null)
            {
                throw new EntityNotFoundException(MessageConstants.GiftMessageConstrant.GiftNotFound(id));
            }
            return gift;
        }
        public async Task<Gift> GetGiftByIdAsync(Guid id)
        {
            List<Expression<Func<Gift, bool>>> filters = new()
            {
                f => f.Id == id,
            };
            var gift = await FirstOrDefaultAsync(filters: filters);
            if (gift == null)
            {
                throw new EntityNotFoundException(MessageConstants.GiftMessageConstrant.GiftNotFound(id));
            }
            return gift;
        }
        private List<Expression<Func<Gift, bool>>> getFiltersFromFGiftFilterRequest(GiftFilterRequest filterRequest)
        {
            List<Expression<Func<Gift, bool>>> filters = new();

            if (filterRequest.Code != null)
            {
                filters.Add(f => f.Code == filterRequest.Code);
            }

            if (filterRequest.Name is { Length: > 0 })
            {
                filters.Add(f => f.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }

            //if (filterRequest.Points > 0)
            //{
            //    filters.Add(f => f.Points >= filterRequest.Points);
            //}

            return filters;
        }
        public async Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest)
        {
            var filters = getFiltersFromFGiftFilterRequest(filterRequest);
            filters.Add(g => g.Status != BaseEntityStatus.Deleted);
            var page = await GetPageAsync<GetGiftResponse>(
                    paginationRequest: paginationRequest, filters: filters);
            return page;
        }
    }
}
