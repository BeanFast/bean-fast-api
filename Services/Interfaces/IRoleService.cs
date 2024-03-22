using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface IRoleService : IBaseService
    {
        Task<Role> GetRoleByRoleNameAsync(RoleName roleName);
        Task<Role> GetRoleByIdAsync(Guid id);
    }
}
