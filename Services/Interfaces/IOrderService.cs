using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface IOrderService : IBaseService
    {
        Task<ICollection<GetOrderResponse>> GetAllAsync(OrderFilterRequest request, User user);
        Task<IPaginable<GetOrderResponse>> GetPageAsync(string? userRole, PaginationRequest request);
        Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id);
        //Task<ICollection<GetOrderResponse>> GetOrdersByCustomerIdAsync(Guid userId);
        Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status);
        Task<Order> GetByIdAsync(Guid id);
        Task<ICollection<Order>> GetOrdersDeliveringByProfileIdAndDelivererId(Guid profileId, Guid delivererId);
        Task CreateOrderAsync(User user, CreateOrderRequest request);
        Task UpdateOrderStatusByQRCodeAsync(string qrCode, Guid delivererId);
        Task UpdateOrderCompleteStatusAsync(Guid orderId);
        Task UpdateOrderDeliveryStatusAsync(Guid orderId);
        Task UpdateOrderCancelStatusAsync(Guid orderId);
        Task FeedbackOrderAsync(Guid orderId, FeedbackOrderRequest request);
        Task DeleteAsync(Guid guid);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest request);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user);
    }
}
