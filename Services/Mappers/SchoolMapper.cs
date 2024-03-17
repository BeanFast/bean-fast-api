using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects.Models.School.Request;
using DataTransferObjects.Models.School.Response;

namespace Services.Mappers
{
    public class SchoolMapper : AutoMapper.Profile
    {
        public SchoolMapper()
        {
            CreateMap<School, GetSchoolIncludeAreaAndLocationResponse>();
            CreateMap<Area, GetSchoolIncludeAreaAndLocationResponse.AreaOfGetSchoolResponse>();
            CreateMap<Location, GetSchoolIncludeAreaAndLocationResponse.LocationOfGetSchoolResponse>();
            CreateMap<CreateSchoolRequest, School>();
        }
    }
}
