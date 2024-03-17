using BeanFastApi.Validators;
using DataTransferObjects.Models.SessionDetail.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net;
using Utilities.Enums;
using Utilities.Exceptions;

namespace BeanFastApi.Controllers
{

    public class SessionDetailsController : BaseController
    {
        private readonly ISessionDetailService _sessionDetailService;
        public SessionDetailsController(ISessionDetailService sessionDetailService)
        {
            _sessionDetailService = sessionDetailService;
        }

        // Get delivery schedule by delivererId
        [HttpGet("deliveryschedule/{delivererId}")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> ViewDeliveryScheduleAsync([FromRoute] Guid delivererId)
        {
            var userId = GetUserId();
            var sessionDetails = await _sessionDetailService.GetSessionDetailByDelivererIdAsync(delivererId, userId);
            return SuccessResult(sessionDetails);
        }

        //Create new session detail (create new delivery schedule)
        [HttpPost]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> CreateSessionDetail([FromBody] CreateSessionDetailRequest request)
        {
            await _sessionDetailService.CreateSessionDetailAsync(request);
            return SuccessResult<object>(statusCode: HttpStatusCode.Created);
        }

        // Update location, session, deliverer
        [HttpPut("update/{id}")]
        [Authorize(RoleName.MANAGER)]
        public async Task<IActionResult> UpdateSessionDetail([FromRoute] Guid id, [FromBody] UpdateSessionDetailRequest request)
        {
            await _sessionDetailService.UpdateSessionDetailByIdAsync(id, request);
            return SuccessResult<object>(statusCode: HttpStatusCode.OK);
        }
    }
}
