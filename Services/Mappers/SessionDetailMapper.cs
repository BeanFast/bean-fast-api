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
            CreateMap<SessionDetail, GetSessionDetailResponse>();
            CreateMap<Order, GetSessionDetailResponse.OrderOfSessionDetail>();
            CreateMap<Session, GetSessionDetailResponse.SessionOfSessionDetail>();
            CreateMap<OrderDetail, GetSessionDetailResponse.OrderOfSessionDetail.OrderDetailOfOrder>();
            CreateMap<Location, GetSessionDetailResponse.LocationOfSessionDetail>();
            CreateMap<School, GetSessionDetailResponse.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Area, GetSessionDetailResponse.LocationOfSessionDetail.SchoolOfLocation.AreaOfSchool>();
        }
    }
}
