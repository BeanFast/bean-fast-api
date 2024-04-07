using BeanFastApi.Validators;
using DataTransferObjects.Models.Notification.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly INotificationService _notificationService;
        public NotificationsController(IUserService userService, INotificationService notificationService) : base(userService)
        {
            _notificationService = notificationService;
        }
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> MarkasReadNotificationsAsync([FromBody] MarkAsReadNotificationRequest request)
        {
            await _notificationService.MarkAsReadNotificationAsync(request, await GetUserAsync());
            return SuccessResult(new { });
        }
        [HttpPost]
        public async Task<IActionResult> SendNotificationsAsync([FromBody] CreateNotificationRequest request)
        {
            await _notificationService.SendNotificationAsync(request);
            return SuccessResult(new { });
        }
    }
}
