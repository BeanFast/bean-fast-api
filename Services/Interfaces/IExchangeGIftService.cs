using BusinessObjects.Models;
using DataTransferObjects.Models.ExchangeGift.Request;
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
        Task CreateExchangeGiftAsync(CreateExchangeGiftRequest request, Guid customerId);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest request);
    }
}
