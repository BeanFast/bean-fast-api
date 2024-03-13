using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.Settings;

namespace Services.Implements
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<Role> GetRoleByRoleNameAsync(RoleName roleName)
        {
            var role = await _repository.FirstOrDefaultAsync(filters: new()
            {
                r => r.EnglishName == roleName.ToString()
            });
            return role!;
        }
    }
}
