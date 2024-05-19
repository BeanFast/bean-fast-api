using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;

namespace Repositories.Interfaces;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<ICollection<Order>> GetCompletedOrderIncludeKitchenAsync();
    Task<ICollection<Order>> GetCompletedOrderIncludeSchoolAsync();
    Task<ICollection<Order>> GetOrdersAsync(DateTime startDate, DateTime endDate, int? status = null);
    Task<ICollection<Order>> GetDeliveringOrdersByDelivererIdAndCustomerIdAsync(Guid delivererId, Guid customerId);
    Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status);
    Task<Order> GetByIdAsync(Guid id);
    Task<ICollection<GetOrderResponse>> GetAllAsync(OrderFilterRequest request, User user);
    Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request);
    Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id);
    Task<IPaginable<GetOrderResponse>> GetPageAsync(PaginationRequest paginationRequest, OrderFilterRequest request, User user);
    Task<ICollection<GetDelivererIdAndOrderCountBySessionDetailIdResponse>> GetDelivererIdAndOrderCountBySessionDetailId(Guid sessionDetailId);
    //Task<GetOrderByIdResponse> GetOderResponseByIdAsync(Guid id);
}