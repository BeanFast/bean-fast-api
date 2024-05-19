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
        Task<IPaginable<GetOrderResponse>> GetPageAsync(PaginationRequest paginationRequest, OrderFilterRequest request, User user);

        Task<GetOrderResponse> GetOderResponseByIdAsync(Guid id);
        //Task<ICollection<GetOrderResponse>> GetOrdersByCustomerIdAsync(Guid userId);
        Task<List<GetOrderResponse>> GetValidOrderResponsesByQRCodeAsync(string qrCode, Guid delivererId);
        Task<ICollection<GetOrderResponse>> GetOrdersByStatusAsync(int status);
        Task<Order> GetByIdAsync(Guid id);
        Task<ICollection<GetOrderResponse>> GetOrdersDeliveringByProfileIdAndDelivererId(Guid profileId, Guid delivererId);
        Task CreateOrderAsync(User user, CreateOrderRequest request);
        //Task UpdateOrderStatusByQRCodeAsync(string qrCode, User deliverer);
        Task UpdateOrderCompleteStatusAsync(Guid orderId, User user);
        Task UpdateOrderDeliveryStatusAsync(Guid orderId);
        Task<ICollection<GetOrdersByLastDaysResponse>> GetOrdersByLastDatesAsync(int numberOfDate);
        //Task UpdateOrderCancelStatusAsync(Guid orderId);
        //Task UpdateOrderCancelStatusAsync(Order orderEntity);
        //Task UpdateOrderCancelStatusForCustomerAsync(Order orderEntity);
        Task UpdateOrderStatusAfterDeliveryTimeEndedAsync();
        Task FeedbackOrderAsync(Guid orderId, FeedbackOrderRequest request);
        Task DeleteAsync(Guid guid);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest request, User user);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user);
        Task CancelOrderAsync(User user, Guid id, CancelOrderRequest cancelOrderRequest);
        Task CancelOrderAsync(Order order, CancelOrderRequest request, User user);
        Task CancelOrderForManagerAsync(Order orderEntity, CancelOrderRequest request, User manager);
        Task CancelOrderForCustomerAsync(Order orderEntity, CancelOrderRequest request, User manager);
        Task<ICollection<GetOrdersByLastMonthsResponse>> GetOrdersByLastMonthsAsync(GetOrdersByLastMonthsRequest request);
        Task<ICollection<GetTopSchoolBestSellerResponse>> GetTopSchoolBestSellers(int topCount);
        Task<ICollection<GetTopBestSellerKitchenResponse>> GetTopBestSellerKitchens(int topCount, bool orderDesc);
    }
}
