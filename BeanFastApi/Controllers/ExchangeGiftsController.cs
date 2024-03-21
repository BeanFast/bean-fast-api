using BeanFastApi.Validators;
using DataTransferObjects.Models.ExchangeGift;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class ExchangeGiftsController : BaseController
    {
        private readonly IExchangeGIftService _exchangeGiftService;

        public ExchangeGiftsController(IExchangeGIftService exchangeGiftService)
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
    }
}
