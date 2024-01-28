using AutoMapper;
using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class CategoryMapper : AutoMapper.Profile
    {
        public CategoryMapper()
        {
            CreateMap<CreateCategoryRequest, Category>();
        }
    }
}
