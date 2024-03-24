using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Notification.Request
{
    public class CreateNotificationRequest
    {
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public string? Link { get; set; }
        public string DeviceToken {  get; set; }
        public ICollection<NotificationDetailOfCreateNotificationRequest> NotificationDetails { get; set; }
        public class NotificationDetailOfCreateNotificationRequest
        {
            public Guid UserId { get; set; }
        }
    }
}
