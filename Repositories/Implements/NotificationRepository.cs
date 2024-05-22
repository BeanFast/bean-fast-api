using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Notification.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Utilities.Statuses;
using Utilities.Utils;

namespace Repositories.Implements;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<IPaginable<GetNotificationResponse>> GetNotificationPageByCurrentUser(PaginationRequest paginationRequest, User user)
    {
        var notificationsPage = await GetPageAsync(paginationRequest, filters: new()
            {
                n => n.NotificationDetails.Any(nd => nd.UserId == user.Id),
                n => n.Status != BaseEntityStatus.Deleted
            },
            include: i => i.Include(n => n.NotificationDetails.Where(nd => nd.UserId == user.Id)),
            orderBy: o => o.OrderByDescending(noti => noti.NotificationDetails.Max(nd => nd.SendDate))
        );
        Paginate<GetNotificationResponse> result = new Paginate<GetNotificationResponse>
        {
            Page = notificationsPage.Page,
            TotalPages = notificationsPage.TotalPages,
            Items = _mapper.Map<List<GetNotificationResponse>>(notificationsPage.Items),
            Size = notificationsPage.Size,
            Total = notificationsPage.Total
        };
        return result;
    }
    public async Task<Notification?> GetUnreadNotificationById(Guid notificationId, Guid userId)
    {

        var notification = await FirstOrDefaultAsync(filters: new()
                {
                    n => n.Id == notificationId && n.Status == BaseEntityStatus.Active
                }, include: i => i.Include(n => n.NotificationDetails.Where(nd => nd.Status == NotificationDetailStatus.Unread && nd.UserId == userId)));
        return notification;
    }

    public async Task<int> CountUnreadNotification(User user)
    {
        var unreadNoti = await GetListAsync(filters: new()
        {
            n => n.NotificationDetails.Any(nd => nd.UserId == user.Id && nd.Status == NotificationDetailStatus.Unread)
        });
        return unreadNoti.Count;
    }
}