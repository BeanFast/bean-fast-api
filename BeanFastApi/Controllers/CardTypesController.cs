using DataTransferObjects.Models.CardType.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class CardTypesController : BaseController
    {
        private readonly ICardTypeService _cardTypeService;

        public CardTypesController(ICardTypeService cardTypeService)
        {
            _cardTypeService = cardTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCardTypesAsync()
        {
            var cardTypes = await _cardTypeService.GetAllAsync();
            return SuccessResult(cardTypes);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCardTypeASync([FromForm] CreateCardTypeRequest request)
        {
            await _cardTypeService.CreateCardTypeAsync(request);
            return SuccessResult<object>();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> CreateCardTypeASync([FromRoute] Guid id, [FromForm] UpdateCardTypeRequest request)
        {
            await _cardTypeService.UpdateCardTypeAsync(request);
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardTypeASync([FromRoute] Guid id)
        {
            //await _cardTypeService.CreateCardTypeAsync(request);
            return SuccessResult<object>();
        }
    }
}
