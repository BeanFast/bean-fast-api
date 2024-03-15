using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        Task<ICollection<GetOrderResponse>> GetAllAsync(string? userRole);
        Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request);
        Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id);
        Task<ICollection<GetOrderResponse>> GetOrdersByProfileIdAsync(Guid profileId, Guid userId);
        Task<Order> GetByIdAsync(Guid id);
        Task CreateOrderAsync(CreateOrderRequest request);
        Task UpdateOrderCookingStatusAsync(Guid foodId);
        Task UpdateOrderCompleteStatusAsync(Guid foodId);
        Task UpdateOrderDeliveryStatusAsync(Guid foodId);
        Task UpdateOrderCancelStatusAsync(Guid foodId);
        Task FeedbackOrderAsync(Guid foodId, FeedbackOrderRequest request);
        Task DeleteAsync(Guid guid);
    }
}
