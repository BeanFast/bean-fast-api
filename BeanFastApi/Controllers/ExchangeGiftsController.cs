using BeanFastApi.Validators;
using DataTransferObjects.Models.ExchangeGift;
using DataTransferObjects.Models.OrderActivity.Request;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        [Authorize(RoleName.CUSTOMER)]
        public async Task<IActionResult> CreateExchangeGiftAsync([FromBody] ExchangeGiftRequest request)
        {
            await _exchangeGiftService.CreateExchangeGiftAsync(request, GetUserId());
            return SuccessResult<object>();
        }
        [HttpGet("{exchangeGiftId}/orderActivities")]
        [Authorize]
        public async Task<IActionResult> GetOrderActivitiesByExchangeGiftIdAsync([FromRoute] Guid exchangeGiftId)
        {
            var user = await GetUserAsync();
            var result =  await _exchangeGiftService.GetOrderActivitiesByExchangeGiftIdAsync(exchangeGiftId, user);
            return SuccessResult(result);
        }
        [HttpPost("{exchangeGiftId}/orderActivities")]
        public async Task<IActionResult> CreateOrderActivitiesAsync([FromRoute] Guid exchangeGiftId,[FromForm] CreateOrderActivityRequest request)
        {
            request.ExchangeGiftId = exchangeGiftId;
            await _exchangeGiftService.CreateOrderActivityAsync(request);
            return SuccessResult<object>();
        }
    }
}
