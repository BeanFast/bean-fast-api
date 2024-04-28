using BusinessObjects.Models;
using DataTransferObjects.Models.Role.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class RoleMapper : AutoMapper.Profile
    {
        public RoleMapper()
        {
            CreateMap<Role, GetRoleResponse>();
        }
    }
}
