using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}