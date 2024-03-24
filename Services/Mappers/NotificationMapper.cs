using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class NotificationMapper : AutoMapper.Profile
    {
        public NotificationMapper()
        {
            CreateMap<CreateNotificationRequest, Notification>();
            CreateMap<CreateNotificationRequest.NotificationDetailOfCreateNotificationRequest, NotificationDetail>();
        }
    }
}
