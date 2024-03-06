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
    }
}
