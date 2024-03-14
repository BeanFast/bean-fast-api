using BusinessObjects.Models;
using DataTransferObjects.Models.User.Response;
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
            CreateMap<User, GetDelivererResponse>();
        }
    }
}
