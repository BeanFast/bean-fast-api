using BusinessObjects.Models;
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
        }
    }
}
