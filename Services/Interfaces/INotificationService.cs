using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificationService : IBaseService 
    {
        Task SendNotificationAsync(CreateNotificationRequest request);

        Task MarkAsReadNotificationAsync(MarkAsReadNotificationRequest request, User user);
    }
}
