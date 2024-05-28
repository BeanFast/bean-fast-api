using BusinessObjects.Models;
using DataTransferObjects.Models.Menu.Response;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class SessionMapper : AutoMapper.Profile
    {
        public SessionMapper()
        {
            CreateMap<Session, GetSessionForDeliveryResponse>();
            CreateMap<SessionDetail, GetSessionForDeliveryResponse.SessionDetailOfSession>().ForMember(src => src.Deliverers, opt => opt.MapFrom(sd => sd.SessionDetailDeliverers.Select(sdd => sdd.Deliverer)));
            CreateMap<Order, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession>();
            CreateMap<User, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession.DelivererOfOrder>();
            CreateMap<Profile, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession.ProfileOfOrderRessponse>();
            CreateMap<OrderDetail, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession.OrderDetailOfGetOrderResponse>();
            CreateMap<Food, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession.OrderDetailOfGetOrderResponse.FoodOfOrderDetail>();
            CreateMap<Category, GetSessionForDeliveryResponse.SessionDetailOfSession.OrderOfSession.OrderDetailOfGetOrderResponse.FoodOfOrderDetail.CategoryOfFood>();
            CreateMap<Location, GetSessionForDeliveryResponse.SessionDetailOfSession.LocationOfSessionDetail>();

            CreateMap<CreateSessionRequest, Session>();
            CreateMap<CreateSessionRequest.SessionDetailOfCreateSessionRequest, SessionDetail>();
        }
    }
}
