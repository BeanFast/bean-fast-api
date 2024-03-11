using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Gift.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Runtime.CompilerServices;

namespace BeanFastApi.Controllers
{
    public class GiftsController : BaseController
    {

        private readonly IGiftService _giftService;

        public GiftsController(IGiftService giftService)
        {
            _giftService = giftService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGiftPageAsync(
            [FromQuery] PaginationRequest paginationRequest, 
            [FromQuery] GiftFilterRequest filterRequest)
        {
            var result = await _giftService.GetGiftPageAsync(paginationRequest, filterRequest);
            return SuccessResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGiftAsync([FromForm] CreateGiftRequest request)
        {
            await _giftService.CreateGiftAsync(request);
            return SuccessResult<object>();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGiftAsync([FromRoute]Guid id, [FromForm] UpdateGiftRequest request)
        {
            await _giftService.UpdateGiftAsync(id, request);
            return SuccessResult<object>();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiftAsync([FromRoute]Guid id)
        {
            await _giftService.DeleteGiftAsync(id);
            return SuccessResult<object>();
        }
    }
}
