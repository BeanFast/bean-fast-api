using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Notification.Response;
using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface INotificationService : IBaseService 
    {
        Task<BatchResponse> SendNotificationAsync(CreateNotificationRequest request);
        Task<int> CountUnreadNotification(User user);
        Task MarkAsReadNotificationAsync(MarkAsReadNotificationRequest request, User user);
        Task<IPaginable<GetNotificationResponse>> GetNotificationPageByCurrentUser(PaginationRequest paginationRequest, User user);
    }
}
