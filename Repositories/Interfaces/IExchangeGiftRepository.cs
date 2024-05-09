using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IExchangeGiftRepository : IGenericRepository<ExchangeGift>
    {
        Task<ExchangeGift> GetByIdAsync(Guid exchangeGiftId);
        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(
            ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user, Guid profileId);
        Task<ICollection<ExchangeGift>> GetDeliveringExchangeGiftsByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId);
        Task<ExchangeGift?> GetByIdIncludeDeliverersAsync(Guid exchangeGiftId);
        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest);

    }
}
