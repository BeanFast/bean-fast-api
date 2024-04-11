using BusinessObjects.Models;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderActivityService : IBaseService
    {
        Task<OrderActivity> GetByIdAsync(Guid id);

        Task<GetOrderActivityResponse> GetOrderActivityResponseByIdAsync(Guid id);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest orderActivity, User user);
        Task CreateOrderActivityAsync(Order order, OrderActivity orderActivity, User user);
        //Task CreateOrderActivityListAsync(List<OrderActivity> orderActivities);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user);
    }
}
