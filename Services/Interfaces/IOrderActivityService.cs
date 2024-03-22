using BusinessObjects.Models;
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
        Task CreateOrderActivityAsync(OrderActivity orderActivity);
        Task CreateOrderActivityListAsync(List<OrderActivity> orderActivities);
    }
}
