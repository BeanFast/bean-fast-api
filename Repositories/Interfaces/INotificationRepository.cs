using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Notification.Response;

namespace Repositories.Interfaces;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<Notification?> GetUnreadNotificationById(Guid notificationId, Guid userId);
    Task<IPaginable<GetNotificationResponse>> GetNotificationPageByCurrentUser(PaginationRequest paginationRequest, User user);
    Task<int> CountUnreadNotification(User user);
}