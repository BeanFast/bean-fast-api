using DataTransferObjects.Models.ExchangeGift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IExchangeGIftService : IBaseService
    {
        Task CreateExchangeGiftAsync(ExchangeGiftRequest request, Guid customerId);
    }
}
