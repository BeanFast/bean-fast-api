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
            CreateMap<School, GetSchoolResponse>();
            CreateMap<Area, GetSchoolResponse.AreaOfGetSchoolResponse>();
            CreateMap<Location, GetSchoolResponse.LocationOfGetSchoolResponse>();
            CreateMap<CreateSchoolRequest, School>();
        }
    }
}
