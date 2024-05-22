using BeanFastApi.Validators;
using DataTransferObjects.Core.Pagination;
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotificationsByCurrentUser([FromQuery] PaginationRequest paginationRequest)
        {
            var notificationPage = await _notificationService.GetNotificationPageByCurrentUser(paginationRequest, await GetUserAsync());
            return SuccessResult(notificationPage);
        }
        [HttpGet("count/unread")]
        [Authorize]
        public async Task<IActionResult> CountNotificationsByCurrentUser()
        {
            var notificationPage = await _notificationService.CountUnreadNotification(await GetUserAsync());
            return SuccessResult(notificationPage);
        }
        [HttpPost]
        public async Task<IActionResult> SendNotificationsAsync([FromBody] CreateNotificationRequest request)
        {
            var response = await _notificationService.SendNotificationAsync(request);
            return SuccessResult(response);
        }
    }
}
