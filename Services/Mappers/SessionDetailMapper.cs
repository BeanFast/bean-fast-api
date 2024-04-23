using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.SessionDetail.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class SessionDetailMapper : AutoMapper.Profile
    {
        public SessionDetailMapper()
        {
            CreateMap<CreateSessionDetailRequest, SessionDetail>();
            CreateMap<UpdateSessionDetailRequest, SessionDetail>();
            CreateMap<SessionDetail, GetSessionDetailResponse>().ForMember(dest => dest.Deliverers, src => src.MapFrom(sd => sd.SessionDetailDeliverers!.Select(sdd => sdd.Deliverer)));
            CreateMap<Order, GetSessionDetailResponse.OrderOfSessionDetail>();
            CreateMap<Session, GetSessionDetailResponse.SessionOfSessionDetail>();
            CreateMap<OrderDetail, GetSessionDetailResponse.OrderOfSessionDetail.OrderDetailOfOrder>();
            CreateMap<Location, GetSessionDetailResponse.LocationOfSessionDetail>();
            CreateMap<School, GetSessionDetailResponse.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Area, GetSessionDetailResponse.LocationOfSessionDetail.SchoolOfLocation.AreaOfSchool>();


            CreateMap<SessionDetail, GetIncommingDeliveringSessionDetails>();
            CreateMap<Order, GetIncommingDeliveringSessionDetails.OrderOfSessionDetail>();
            CreateMap<OrderDetail, GetIncommingDeliveringSessionDetails.OrderOfSessionDetail.OrderDetailOfOrder>();
            CreateMap<Location, GetIncommingDeliveringSessionDetails.LocationOfSessionDetail>();
            CreateMap<School, GetIncommingDeliveringSessionDetails.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Area, GetIncommingDeliveringSessionDetails.LocationOfSessionDetail.SchoolOfLocation.AreaOfSchool>();
        }
    }
}
