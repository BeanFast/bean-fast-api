using BusinessObjects.Models;
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
            CreateMap<SessionDetail, GetSessionDetailResponse>();
        }
    }
}
