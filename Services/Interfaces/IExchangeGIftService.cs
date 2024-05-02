using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.ExchangeGift.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IExchangeGIftService : IBaseService
    {
        Task CreateExchangeGiftAsync(CreateExchangeGiftRequest request, User user);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest request, User user);
        Task<ExchangeGift> GetByIdAsync(Guid exchangeGiftId);
        Task CancelExchangeGiftForCustomerAsync(ExchangeGift exchangeGift, CancelExchangeGiftRequest request, User customer);
        Task CancelExchangeGiftForManagerAsync(ExchangeGift exchangeGift, CancelExchangeGiftRequest request, User manager);
        Task UpdateExchangeGiftToDeliveryStatusAsync(Guid exchangeGiftId);
        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest);
        Task<List<GetExchangeGiftResponse>> GetValidExchangeGiftResponsesByQRCodeAsync(string qrCode, Guid delivererId);
        Task<GetExchangeGiftResponse> GetExchangeGiftResponseByIdAsync(Guid id);
        Task CancelExchangeGiftAsync(Guid exchangeGiftId, CancelExchangeGiftRequest request, User user);
        Task UpdateExchangeGiftCompleteStatusAsync(Guid exchangeGiftId, User deliverer);
        Task<IPaginable<GetExchangeGiftResponse>> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(ExchangeGiftFilterRequest filterRequest, PaginationRequest paginationRequest, User user, Guid profileId);
    }
}
