using BeanFastApi.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Utilities.Enums;

namespace BeanFastApi.Controllers
{

    public class SessionDetailsController : BaseController
    {
        private readonly ISessionDetailService _sessionDetailService;
        public SessionDetailsController(ISessionDetailService sessionDetailService)
        {
            _sessionDetailService = sessionDetailService;
        }

        [HttpGet("deliveryschedule/{delivererId}")]
        [Authorize(RoleName.DELIVERER)]
        public async Task<IActionResult> ViewDeliveryScheduleAsync([FromRoute] Guid delivererId)
        {
            var userId = GetUserId();
            var sessionDetails = await _sessionDetailService.GetSessionDetailByDelivererIdAsync(delivererId, userId);
            return SuccessResult(sessionDetails);
        }

        /*
        DTO: session detail | session | location | deliverer (user)
        Service: View session detail | Update session detail (session | location | user)

        chưa lấy được school, menu
        chưa lấy đúng đơn hàng theo từng deliverer 
        */
    }
}
