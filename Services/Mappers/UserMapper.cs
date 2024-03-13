using BusinessObjects.Models;
using DataTransferObjects.Models.Auth.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class UserMapper : AutoMapper.Profile
    {
        public UserMapper()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}
