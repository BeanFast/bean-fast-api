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
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class RoleService : BaseService<Role>, IRoleService
    {
        public RoleService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            var result = await _repository.FirstOrDefaultAsync(BaseEntityStatus.Active, filters: new()
            {
                r => r.Id == id
            });
            if (result == null) { throw new EntityNotFoundException(message: MessageConstants.AuthorizationMessageConstrant.RoleNotFound); }
            return result;
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
