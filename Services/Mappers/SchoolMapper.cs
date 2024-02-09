using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects.Models.School.Response;

namespace Services.Mappers
{
    public class SchoolMapper : AutoMapper.Profile
    {
        public SchoolMapper()
        {
            CreateMap<School, GetSchoolResponse>();
        }
    }
}
