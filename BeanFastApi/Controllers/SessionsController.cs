using BeanFastApi.Validators;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.User.Response;
using Microsoft.AspNetCore.Mvc;
using Services.Implements;
using Services.Interfaces;
using System.Net;
using Utilities.Enums;

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
        [HttpGet("deliverers/available/{sessionDetailId}")]
        public async Task<IActionResult> GetAvailableDelivererInSessionDeliveryTime(Guid sessionDetailId)
        {
            var result = await _sessionService.GetAvailableDelivererInSessionDeliveryTime(sessionDetailId);
            return SuccessResult(result);
        }
        [HttpGet("{sessionId}/check/{profileId}/{menuDetailId}")]
        public async Task<IActionResult> CheckOrderable([FromRoute] Guid sessionId, [FromRoute] Guid profileId, [FromRoute] Guid menuDetailId)
        {
            var result = await _sessionService.CheckOrderable(menuDetailId, profileId, sessionId);
            return SuccessResult(result);
        }
        [HttpPost]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> CreateSessionAsync([FromBody] CreateSessionRequest request)
        {
            await _sessionService.CreateSessionAsync(request, await GetUserAsync());
            return SuccessResult<object>(null);
        }
        //[HttpPut("{id}")]
        [HttpPut("sessionDetails/{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> UpdateSessionDetail([FromRoute] Guid id, [FromBody] UpdateSessionDetailRequest request)
        {
            await _sessionService.UpdateSessionDetailByIdAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }
        [HttpDelete("{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> DeleteSessionAsync([FromRoute] Guid id)
        {
            await _sessionService.DeleteAsync(id, await GetUserAsync());
            return SuccessResult<object>(null);
        }
    }
}
