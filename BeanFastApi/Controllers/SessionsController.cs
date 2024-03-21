using DataTransferObjects.Models.Session.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class SessionsController : BaseController
    {
        private readonly ISessionService _sessionService;
        public SessionsController(ISessionService sessionService, IUserService userService) : base(userService)
        {
            _sessionService = sessionService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllASync([FromQuery] SessionFilterRequest request)
        {
            var result = await _sessionService.GetAllAsync(GetUserRole(), request);
            return SuccessResult(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateSessionAsync([FromBody] CreateSessionRequest request)
        {
            await _sessionService.CreateSessionAsync(request);
            return SuccessResult<object>(null);
        }
    }
}
