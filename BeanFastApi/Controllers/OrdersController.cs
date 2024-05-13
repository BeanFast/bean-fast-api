﻿using BeanFastApi.Validators;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Core.Response;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.OrderActivity.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using System.Net;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Utils;

namespace BeanFastApi.Controllers
{

    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService, IUserService userService) : base(userService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        [Authorize(RoleName.MANAGER, RoleName.CUSTOMER, RoleName.DELIVERER)]
        public async Task<IActionResult> GetAllOrdersAsync(
            [FromQuery] OrderFilterRequest request,
            [FromQuery] PaginationRequest paginationRequest
            )
        {
            object orders;
            var user = await GetUserAsync();
            
            if (paginationRequest is { Page: 0, Size: 0 })
            {
                orders = await _orderService.GetAllAsync(request, user);

            }
            else
            {
                orders = await _orderService.GetPageAsync(paginationRequest, request, user);
            }
            return SuccessResult(orders);
        }
        [HttpGet("{id}")]
        [Authorize(RoleName.MANAGER, RoleName.CUSTOMER, RoleName.DELIVERER)]
        public async Task<IActionResult> GetOrderByIdAsync([FromRoute] Guid id)
        {
            GetOrderByIdResponse order;
            order = await _orderService.GetOderResponseByIdAsync(id);
            return SuccessResult(order);
        }

        [HttpGet("{orderId}/orderActivities")]
        [Authorize]
        public async Task<IActionResult> GetOrderActivitiesByOrderIdAsync([FromRoute] Guid orderId)
        {
            var user = await GetUserAsync();
            var result = await _orderService.GetOrderActivitiesByOrderIdAsync(orderId, user);
            return SuccessResult(result);
        }
        [HttpGet("countByMonth")]
        [Authorize(RoleName.MANAGER, RoleName.ADMIN)]
        public async Task<IActionResult> GetOrdersByLastMonthsAsync([FromQuery] GetOrdersByLastMonthsRequest request)
        {
            var result = await _orderService.GetOrdersByLastMonthsAsync(request);
            return SuccessResult(result);
        }
        [HttpGet("countByDay")]
        [Authorize(RoleName.MANAGER, RoleName.ADMIN)]
        public async Task<IActionResult> GetOrdersByLastDateAsync([FromQuery] int dateCount = 7)
        {
            var result = await _orderService.GetOrdersByLastDatesAsync(dateCount);
            return SuccessResult(result);
        }
        [HttpGet("schools/bestSellers")]
        [Authorize(RoleName.MANAGER, RoleName.ADMIN)]
        public async Task<IActionResult> GetTopSchoolBestSellers([FromQuery] int topCount = 10)
        {
            var result = await _orderService.GetTopSchoolBestSellers(topCount);
            return SuccessResult(result);
        }
        [HttpGet("kitchens/bestSellers")]
        [Authorize(RoleName.MANAGER, RoleName.ADMIN)]
        public async Task<IActionResult> GetTopBestSellerKitchens([FromQuery] int topCount = 10, [FromQuery] bool desc = false)
        {
            
            var result = await _orderService.GetTopBestSellerKitchens(topCount, desc);
            return SuccessResult(result);
        }
        //[HttpGet("status/{status}")]
        //[Authorize]
        //public async Task<IActionResult> GetOrdersByStatus(int status)
        //{
        //    var orders = await _orderService.GetOrdersByStatusAsync(status);
        //    return SuccessResult(orders);
        //}

        [HttpPost]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            var user = await GetUserAsync();
            await _orderService.CreateOrderAsync(user, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }
        [HttpPost("{orderId}/orderActivities")]
        [Authorize]
        public async Task<IActionResult> CreateOrderActivityAsync([FromRoute] Guid orderId, [FromForm] CreateOrderActivityRequest request)
        {
            request.OrderId = orderId;
            await _orderService.CreateOrderActivityAsync(request, await GetUserAsync());
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }
        [HttpPut("cancel/{id}")]
        [Authorize(RoleName.CUSTOMER, RoleName.MANAGER)]
        public async Task<IActionResult> CancelOrderAsync([FromRoute] Guid id, [FromForm] CancelOrderRequest cancelOrderRequest)
        {
            var user = await GetUserAsync();
            await _orderService.CancelOrderAsync(user, id, cancelOrderRequest);
            return SuccessResult<object>();
        }
        [HttpPut("complete/{id}")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> ChangeOrderStatusToCompleteAsync([FromRoute] Guid id)
        {
            var user = await GetUserAsync();
            await _orderService.UpdateOrderCompleteStatusAsync(id, user);

            return SuccessResult<object>();

        }
        //[HttpPut("delivering/{id}")]
        //public async Task<IActionResult> DeliveringOrderAsync([FromRoute] Guid id)
        //{
        //    var user = await GetUserAsync();
        //    await _orderService.UpdateOrderDeliveryStatusAsync(id);
        //    return SuccessResult<object>();
        //}

        //[HttpPut("{id}")]
        //[Authorize]
        //public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id)
        //{
        //    var realTime = TimeUtil.GetCurrentVietNamTime();
        //    var order = await _orderService.GetByIdAsync(id);
        //    var user = await GetUserAsync();

        //    if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName) && order.Status == OrderStatus.Cooking)
        //    {
        //        await _orderService.UpdateOrderDeliveryStatusAsync(id);
        //    }
        //    else if (RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName))
        //    {
        //        //if (order.Status == OrderStatus.Delivering)
        //        //    await _orderService.UpdateOrderCompleteStatusAsync(id);
        //        //else if (order.Status == OrderStatus.Completed)
        //    }
        //    else if (order.Status == OrderStatus.Delivering && realTime > order.SessionDetail!.Session!.DeliveryEndTime)
        //    {
        //        await _orderService.UpdateOrderStatusAfterDeliveryTimeEndedAsync();
        //    }
        //    else
        //    {
        //        throw new InvalidRoleException();
        //    }

        //    return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        //}



        //[HttpGet("{customerId}")]
        //[Authorize(RoleName.CUSTOMER)]
        //public async Task<IActionResult> GetOrdersByProfileId([FromRoute] Guid customerId)
        //{
        //    var user  = await GetUser();
        //    var orders = await _orderService.GetOrdersByCustomerIdAsync(user.Id);
        //    return SuccessResult(orders);
        //}

        [HttpGet("getOrderByQrCode")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> GetValidOrdersByQRCode([FromQuery] string qrCode)
        {
            var user = await GetUserAsync();
            var delivererId = user.Id;
            var orders = await _orderService.GetValidOrderResponsesByQRCodeAsync(qrCode, delivererId);
            return SuccessResult(orders);
        }

        //[HttpPut("updateOrderStatusByQrCode")]
        //[Authorize(RoleName.DELIVERER)]
        //public async Task<IActionResult> UpdateOrderStatusByQRCode([FromQuery] string qrCode)
        //{
        //    var user = await GetUserAsync();
        //    await _orderService.UpdateOrderStatusByQRCodeAsync(qrCode, user);
        //    return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        //}

        [HttpPut("{orderId}/feedbacks")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> FeedbackOrderAsync([FromRoute] Guid id, [FromBody] FeedbackOrderRequest request)
        {
            await _orderService.FeedbackOrderAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

    }


}
