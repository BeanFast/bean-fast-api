using BusinessObjects.Models;
using DataTransferObjects.Models.OrderActivity.Response;

namespace Repositories.Interfaces;

public interface IOrderActivityRepository : IGenericRepository<OrderActivity>
{
    Task<OrderActivity> GetByIdAsync(Guid id);
    Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user);
    Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByExchangeGiftIdAsync(Guid exchangeGiftId, User user);
}