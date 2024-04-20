using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Request;
using DataTransferObjects.Models.Location.Response;
using DataTransferObjects.Models.School.Response;
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
            CreateMap<Location, GetLocationIncludeSchoolResponse>();
            CreateMap<Location, GetLocationResponse>();
            CreateMap<Location, GetBestSellerLocationResponse>().ForMember(dest => dest.OrderCount, src => src.MapFrom(l => l.SessionDetails!.Sum(s => s.Orders!.Count())));
            CreateMap<CreateLocationRequest, Location>();
            CreateMap<UpdateLocationRequest, Location>();
            CreateMap<School, GetSchoolResponse>();
        }
    }
}
