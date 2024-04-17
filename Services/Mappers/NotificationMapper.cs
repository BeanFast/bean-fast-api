using BusinessObjects.Models;
using DataTransferObjects.Models.Notification.Request;
using DataTransferObjects.Models.Notification.Response;
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
            CreateMap<Notification, GetNotificationResponse>()
                .ForMember(dest => dest.ReadDate, src => src.MapFrom(src => src.NotificationDetails.First().SendDate))
                .ForMember(dest => dest.SendDate, src => src.MapFrom(src => src.NotificationDetails.First().SendDate))
                .ForMember(dest => dest.UserId, src => src.MapFrom(src => src.NotificationDetails.First().UserId))
                .ForMember(dest => dest.Status, src => src.MapFrom(src => src.NotificationDetails.First().Status));
        }
    }
}
