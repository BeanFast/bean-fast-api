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
        public SessionDetailsController(ISessionDetailService sessionDetailService, IUserService userService) : base(userService)
        {
            _sessionDetailService = sessionDetailService;
        }

        // Get delivery schedule by delivererId
        [HttpGet("deliverySchedule")]
        [Authorize(RoleName.DELIVERER, RoleName.MANAGER)]
        public async Task<IActionResult> ViewDeliveryScheduleAsync([FromQuery] GetSessionDetailFilterRequest request)
        {
            var user = await GetUserAsync();
            var sessionDetails = await _sessionDetailService.GetSessionDetailsAsync(user, request);
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
        
    }
}
