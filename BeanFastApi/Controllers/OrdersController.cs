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
        [Authorize(RoleName.MANAGER, RoleName.CUSTOMER, RoleName.DELIVERER)]
        [ProducesResponseType(typeof(SuccessApiResponse<IPaginable<GetOrderResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllOrders([FromQuery]OrderFilterRequest request)
        {
            object orders;
            var user = await GetUser();
            orders = await _orderService.GetAllAsync(request, user);
            return SuccessResult(orders);
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var user = await GetUser();
            await _orderService.CreateOrderAsync(user, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            var user = await GetUser();

            if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName) && order.Status == OrderStatus.Cooking)
            {
                await _orderService.UpdateOrderDeliveryStatusAsync(id);
            }
            else if (RoleName.MANAGER.ToString().Equals(user.Role!.EnglishName) && order.Status == OrderStatus.Cooking)
            {
                await _orderService.UpdateOrderCancelStatusAsync(id);
            }else if (RoleName.CUSTOMER.ToString().Equals(user.Role!.EnglishName) && order.Status == OrderStatus.Cooking)
            {
                await _orderService.UpdateOrderCancelStatusAsync(id);
            }
            else if (RoleName.DELIVERER.ToString().Equals(user.Role!.EnglishName) && order.Status == OrderStatus.Delivering)
            {
                await _orderService.UpdateOrderCompleteStatusAsync(id);
            }
            else
            {
                throw new InvalidRoleException();
            }

            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

        //[HttpGet("{customerId}")]
        //[Authorize(RoleName.CUSTOMER)]
        //public async Task<IActionResult> GetOrdersByProfileId([FromRoute] Guid customerId)
        //{
        //    var user  = await GetUser();
        //    var orders = await _orderService.GetOrdersByCustomerIdAsync(user.Id);
        //    return SuccessResult(orders);
        //}

        [HttpPut("feedbacks/{orderId}")]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> FeedbackOrder([FromRoute] Guid id, [FromBody] FeedbackOrderRequest request)
        {
            await _orderService.FeedbackOrderAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }

    }


}
