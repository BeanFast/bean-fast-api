using BusinessObjects.Models;
using DataTransferObjects.Models.Area.Response;
using DataTransferObjects.Models.Category.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class AreaMapper : AutoMapper.Profile
    {
        public AreaMapper()
        {
            CreateMap<Area, SearchAreaResponse>();
            CreateMap<School, SearchAreaResponse.SchoolOfSearchAreaResponse>();
        }
    }
}
