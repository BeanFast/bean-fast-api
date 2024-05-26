using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
    public class ExchangeGiftRepository : GenericRepository<ExchangeGift>, IExchangeGiftRepository
    {
        public ExchangeGiftRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public List<Expression<Func<ExchangeGift, bool>>> GetFilterFromFilterRequest(ExchangeGiftFilterRequest filterRequest)
        {
            List<Expression<Func<ExchangeGift, bool>>> expressions = new List<Expression<Func<ExchangeGift, bool>>>();
            if (filterRequest.Status != null)
            {
                if (filterRequest.Status == ExchangeGiftStatus.Cancelled)
                {
                    expressions.Add(eg => eg.Status == ExchangeGiftStatus.Cancelled || eg.Status == ExchangeGiftStatus.CancelledByCustomer);
                }
                else
                {
                    expressions.Add(eg => eg.Status == filterRequest.Status);
                }
            }
            if (!filterRequest.Code.IsNullOrEmpty())
            {
                expressions.Add(eg => eg.Code == filterRequest.Code);
            }
            return expressions;
        }
        public async Task<ExchangeGift> GetByIdAsync(Guid exchangeGiftId)
        {
            var filters = new List<Expression<Func<ExchangeGift, bool>>>
            {
                (exchangeGift) => exchangeGift.Id == exchangeGiftId
            };
            var result = await FirstOrDefaultAsync(filters: filters,
                include: i => i
                .Include(eg => eg.Profile!)
                    .ThenInclude(p => p.User!)
                .Include(o => o.SessionDetail!)
                    .ThenInclude(o => o.Session!)
                .Include(o => o.Activities!)
                ) ?? throw new EntityNotFoundException(MessageConstants.ExchangeGiftMessageConstrant.ExchangeGiftNotFound(exchangeGiftId));
            return result!;
        }
        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user)
        {
            var filters = GetFilterFromFilterRequest(filterRequest);
            filters.Add(ex => ex.Profile!.UserId == user.Id);
            return await GetPageAsync<GetExchangeGiftResponse>(filters: filters, paginationRequest: paginationRequest, orderBy: o => o.OrderByDescending(eg => eg.CreatedDate));
        }
        public async Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user, Guid profileId)
        {
            var filters = GetFilterFromFilterRequest(filterRequest);
            filters.Add(eg => eg.ProfileId == profileId);
            filters.Add(ex => ex.Profile!.UserId == user.Id);
            IPaginable<GetExchangeGiftResponse> page = await GetPageAsync<GetExchangeGiftResponse>(filters: filters, paginationRequest: paginationRequest, orderBy: o => o.OrderByDescending(eg => eg.CreatedDate));
            return page;
        }
        public async Task<ICollection<ExchangeGift>> GetDeliveringExchangeGiftsByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId)
        {
            List<Expression<Func<ExchangeGift, bool>>> filters = new()
            {
                (order) => order.SessionDetail!.SessionDetailDeliverers!.Any(sdd => sdd.DelivererId == delivererId)
                && order.Profile!.UserId == customerId
                && order.Status == ExchangeGiftStatus.Delivering
            };
            var exchangeGifts = await GetListAsync(
                filters: filters,
                include: queryable => queryable
                .Include(o => o.SessionDetail!)
                    .ThenInclude(sd => sd.Session!)
                .Include(o => o.Gift!)
                .Include(e => e.Profile!)
                    .ThenInclude(p => p.User!)
            );
            return exchangeGifts!;
        }
        public async Task<ExchangeGift?> GetByIdIncludeDeliverersAsync(Guid exchangeGiftId)
        {
            var filters = new List<Expression<Func<ExchangeGift, bool>>>
            {
                (exchangeGift) => exchangeGift.Id == exchangeGiftId
            };
            var result = await FirstOrDefaultAsync(filters: filters,
                include: i => i
                .Include(eg => eg.Profile!)
                    .ThenInclude(p => p.User!)
                .Include(o => o.SessionDetail!)
                    .ThenInclude(sd => sd.SessionDetailDeliverers!)
                    .ThenInclude(sdd => sdd.Deliverer!)
                .Include(o => o.SessionDetail!)
                    .ThenInclude(o => o.Session!)
                .Include(o => o.Activities!)
                );
            return result;
        }
    }
}
