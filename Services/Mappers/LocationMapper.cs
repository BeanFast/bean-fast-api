using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class LocationMapper : AutoMapper.Profile
    {
        public LocationMapper()
        {
            CreateMap<Location, GetLocationResponse>();
        }
    }
}
