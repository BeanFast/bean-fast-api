using BusinessObjects.Models;
using DataTransferObjects.Models.User.Response;
using DataTransferObjects.Models.Auth.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.Models.User.Request;

namespace Services.Mappers
{
    public class UserMapper : AutoMapper.Profile
    {
        public UserMapper()
        {
            CreateMap<User, GetDelivererResponse>();
            CreateMap<User, GetCurrentUserResponse>();
            CreateMap<User, GetUserResponse>().ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role!.Name));
            CreateMap<Role, GetUserResponse.RoleOfGetUserResponse>();
            CreateMap<RegisterRequest, User>();
            CreateMap<CreateUserRequest, User>();
        }
    }
}
