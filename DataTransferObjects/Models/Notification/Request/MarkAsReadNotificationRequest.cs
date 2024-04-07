using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Notification.Request
{
    public class MarkAsReadNotificationRequest
    {
        public ICollection<Guid> NotificationIds { get; set; }
    }
}
