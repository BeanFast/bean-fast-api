using DataTransferObjects.Models.Notification.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace BeanFastApi.Controllers
{
    public class TestsController : BaseController
    {
        private readonly INotificationService _notificationService;
        public TestsController(IUserService userService, INotificationService notificationService) : base(userService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> SendNotificationAsync([FromBody] CreateNotificationRequest request)
        {
            await _notificationService.SendNotificationAsync(request);
            return SuccessResult(new object());
        }
    }
}
