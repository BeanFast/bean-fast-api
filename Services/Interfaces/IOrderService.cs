﻿using BusinessObjects.Models;
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
        Task CreateOrderAsync(User user, CreateOrderRequest request);
        //Task UpdateOrderCookingStatusAsync(Guid foodId);
        Task UpdateOrderCompleteStatusAsync(Guid foodId);
        Task UpdateOrderDeliveryStatusAsync(Guid foodId);
        Task UpdateOrderCancelStatusAsync(Guid foodId);
        Task FeedbackOrderAsync(Guid foodId, FeedbackOrderRequest request);
        Task DeleteAsync(Guid guid);
        Task CreateOrderActivityAsync(CreateOrderActivityRequest request);
        Task<ICollection<GetOrderActivityResponse>> GetOrderActivitiesByOrderIdAsync(Guid orderId, User user);
    }
}
