using BeanFastApi.Validators;
using DataTransferObjects.Models.CardType.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class CardTypesController : BaseController
    {
        private readonly ICardTypeService _cardTypeService;

        public CardTypesController(ICardTypeService cardTypeService, IUserService userService) : base(userService)
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
        [Authorize(RoleName.MANAGER)]

        public async Task<IActionResult> CreateCardTypeAsync([FromForm] CreateCardTypeRequest request)
        {
            await _cardTypeService.CreateCardTypeAsync(request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpPut("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> UpdateCardTypeAsync([FromRoute] Guid id, [FromForm] UpdateCardTypeRequest request)
        {
            await _cardTypeService.UpdateCardTypeAsync(id, request, await GetUserAsync());
            return SuccessResult<object>();
        }
        [HttpDelete("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> DeleteCardTypeAsync([FromRoute] Guid id)
        {
            await _cardTypeService.DeleteCardTypeAsync(id, await GetUserAsync());
            return SuccessResult<object>();
        }
    }
}
