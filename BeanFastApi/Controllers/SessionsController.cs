using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.User.Response;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdASync([FromRoute] Guid id, [FromQuery] SessionFilterRequest request)
        {
            var result = await _sessionService.GetSessionForDeliveryResponseByIdAsync(id, request, GetUserRole());
            return SuccessResult(result);
        }
        [HttpGet("deliverers/available/{sessionId}")]
        public async Task<IActionResult> GetAvailableDelivererInSessionDeliveryTime(Guid sessionId)
        {
            var result = await _sessionService.GetAvailableDelivererInSessionDeliveryTime(sessionId);
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
