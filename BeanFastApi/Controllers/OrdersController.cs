using BeanFastApi.Validators;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Core.Response;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;
using Utilities.Constants;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{

    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        [ProducesResponseType(typeof(SuccessApiResponse<IPaginable<GetOrderResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrders()
        {
            object orders;
            var userRole = GetUserRole();
            orders = await _orderService.GetAllAsync(userRole);
            return SuccessResult(orders);
        }

        [HttpPost]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            await _orderService.CreateOrderAsync(request);
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var userRole = GetUserRole();

            if (userRole!.Equals(RoleName.MANAGER.ToString()) && order.Status == (int)OrderStatus.COOKING)
            {
                await _orderService.UpdateOrderDeliveryStatusAsync(id);
            }
            else if (userRole!.Equals(RoleName.CUSTOMER.ToString()) && order.Status == (int)OrderStatus.PENDING
                || userRole!.Equals(RoleName.MANAGER.ToString()) && order.Status == (int)OrderStatus.COOKING)
            {
                await _orderService.UpdateOrderCancelStatusAsync(id);
            }
            else if (userRole!.Equals(RoleName.DELIVERER.ToString()) && order.Status == (int)OrderStatus.DELIVERING)
            {
                await _orderService.UpdateOrderCompleteStatusAsync(id);
            }

            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

        [HttpPut("{id}/feedback")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> FeedbackOrder([FromRoute] Guid id, [FromBody] FeedbackOrderRequest request)
        {
            await _orderService.FeedbackOrderAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

    }


}
