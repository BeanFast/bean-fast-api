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
using Utilities.Exceptions;

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
        [Authorize]
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
        public async Task<IActionResult> CreateOrder(Guid customerId, Guid menuDetailId, [FromBody] CreateOrderRequest request)
        {
            await _orderService.CreateOrderAsync(customerId, menuDetailId, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var userRole = GetUserRole();

            if (userRole!.Equals(RoleName.MANAGER.ToString()) && order.Status == OrderStatus.Pending)
            {
                await _orderService.UpdateOrderCookingStatusAsync(id);
            }
            else if (userRole!.Equals(RoleName.MANAGER.ToString()) && order.Status == OrderStatus.Cooking)
            {
                await _orderService.UpdateOrderDeliveryStatusAsync(id);
            }
            else if (userRole!.Equals(RoleName.CUSTOMER.ToString()) && order.Status == OrderStatus.Pending
                || userRole!.Equals(RoleName.MANAGER.ToString()) && order.Status == OrderStatus.Cooking)
            {
                await _orderService.UpdateOrderCancelStatusAsync(id);
            }
            else if (userRole!.Equals(RoleName.DELIVERER.ToString()) && order.Status == OrderStatus.Delivering)
            {
                await _orderService.UpdateOrderCompleteStatusAsync(id);
            }
            else
            {
                throw new InvalidRoleException();
            }

            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

        [HttpGet("{profileId}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> GetOrdersByProfileId([FromRoute] Guid profileId)
        {
            var userId = GetUserId();
            var orders = await _orderService.GetOrdersByProfileIdAsync(profileId, userId);
            return SuccessResult(orders);
        }

        [HttpPut("feedbacks/{orderId}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> FeedbackOrder([FromRoute] Guid id, [FromBody] FeedbackOrderRequest request)
        {
            await _orderService.FeedbackOrderAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

    }


}
