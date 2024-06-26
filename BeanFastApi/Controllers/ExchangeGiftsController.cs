﻿using BeanFastApi.Validators;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.ExchangeGift.Request;
using DataTransferObjects.Models.OrderActivity.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class ExchangeGiftsController : BaseController
    {
        private readonly IExchangeGIftService _exchangeGiftService;

        public ExchangeGiftsController(IExchangeGIftService exchangeGiftService, IUserService userService) : base(userService)
        {
            _exchangeGiftService = exchangeGiftService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExchangeGiftsAsync(
            [FromQuery] ExchangeGiftFilterRequest filterRequest,
            [FromQuery] PaginationRequest paginationRequest
            )
        {
            var result = await _exchangeGiftService.GetExchangeGiftsAsync(filterRequest, paginationRequest, await GetUserAsync());
            return SuccessResult(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeGiftByIdAsync(
            [FromRoute] Guid id
            )
        {
            var result = await _exchangeGiftService.GetExchangeGiftResponseByIdAsync(id);
            return SuccessResult(result);
        }
        //[HttpGet("profiles/{profileId}")]
        //public async Task<IActionResult> GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(
        //    [FromQuery] ExchangeGiftFilterRequest filterRequest,
        //    [FromQuery] PaginationRequest paginationRequest,
        //    [FromRoute] Guid profileId
        //    )
        //{
        //    var result = await _exchangeGiftService.GetExchangeGiftsByCurrentCustomerAndProfileIdAsync(filterRequest, paginationRequest, await GetUserAsync(), profileId);
        //    return SuccessResult(result);
        //}
        [HttpGet("{exchangeGiftId}/orderActivities")]
        [Authorize]
        public async Task<IActionResult> GetOrderActivitiesByExchangeGiftIdAsync([FromRoute] Guid exchangeGiftId)
        {
            var user = await GetUserAsync();
            var result = await _exchangeGiftService.GetOrderActivitiesByExchangeGiftIdAsync(exchangeGiftId, user);
            return SuccessResult(result);
        }
        [HttpGet("getExchangeGiftByQrCode")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> GetValidExchangeGiftByQRCode([FromQuery] string qrCode)
        {
            var user = await GetUserAsync();
            var delivererId = user.Id;
            var exchangeGifts = await _exchangeGiftService.GetValidExchangeGiftResponsesByQRCodeAsync(qrCode, delivererId);
            return SuccessResult(exchangeGifts);
        }
        [HttpPost("{exchangeGiftId}/orderActivities")]
        public async Task<IActionResult> CreateOrderActivitiesAsync([FromRoute] Guid exchangeGiftId, [FromForm] CreateOrderActivityRequest request)
        {
            request.ExchangeGiftId = exchangeGiftId;
            await _exchangeGiftService.CreateOrderActivityAsync(request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpPost]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateExchangeGiftAsync([FromBody] CreateExchangeGiftRequest request)
        {
            await _exchangeGiftService.CreateExchangeGiftAsync(request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpPut("cancel/{id}")]
        [Authorize(RoleName.CUSTOMER, RoleName.MANAGER)]
        public async Task<IActionResult> CancelExchangeGiftAsync([FromRoute] Guid id, [FromBody] CancelExchangeGiftRequest request)
        {
            await _exchangeGiftService.CancelExchangeGiftAsync(id, request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpPut("complete/{id}")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> CompleteExchangeGiftAsync([FromRoute] Guid id)
        {
            await _exchangeGiftService.UpdateExchangeGiftCompleteStatusAsync(id, await GetUserAsync());
            return SuccessResult<object>();
        }

    }
}
