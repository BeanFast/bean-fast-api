using BeanFastApi.Validators;
using DataTransferObjects.Models.Game.Request;
using DataTransferObjects.Models.Game.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{
    public class GamesController : BaseController
    {
        private readonly IGameService _gameService;
        public GamesController(IUserService userService, IGameService gameService) : base(userService)
        {
            _gameService = gameService;
        }
        [HttpGet]
        public async Task<IActionResult> GetGamesAsync()
        {
            return SuccessResult(await _gameService.GetGamesAsync());
        }
        [HttpPost]
        [Authorize(RoleName.MANAGER, RoleName.ADMIN)]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameRequest request)
        {
            await _gameService.CreateGameAsync(request, await GetUserAsync());
            return SuccessResult<object>();
        }
    }
}
