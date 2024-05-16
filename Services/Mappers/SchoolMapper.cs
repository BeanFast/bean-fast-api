using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;
using Utilities.Utils;

namespace Services.Mappers
{
    public class SchoolMapper : AutoMapper.Profile
    {
        public SchoolMapper()
        {
            CreateMap<School, GetSchoolIncludeAreaAndLocationResponse>().ForMember(src => src.Orderable, opt => opt.MapFrom(s => s.Locations!.Any(l => l.SessionDetails!.Any(sd => sd.Session!.OrderStartTime <= TimeUtil.GetCurrentVietNamTime() && sd.Session.OrderEndTime > TimeUtil.GetCurrentVietNamTime()))));
            CreateMap<Area, GetSchoolIncludeAreaAndLocationResponse.AreaOfGetSchoolResponse>();
            CreateMap<Location, GetSchoolIncludeAreaAndLocationResponse.LocationOfGetSchoolResponse>();
            CreateMap<CreateSchoolRequest, School>();
        }
    }
}
