using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using DataTransferObjects.Models.Order.Response;
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
//<<<<<<< HEAD
//        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest);
//        Task<ICollection<GetDelivererIdAndOrderCountBySessionDetailIdResponse>> GetDelivererIdAndOrderCountBySessionDetailId(Guid sessionDetailId);
//=======
        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user);
        Task<ICollection<GetDelivererIdAndOrderCountBySessionDetailIdResponse>> GetDelivererIdAndOrderCountBySessionDetailId(Guid sessionDetailId);
//>>>>>>> c1cbe8bf578b78aef3a6bb4fbfa373b636d2195d

    }
}
