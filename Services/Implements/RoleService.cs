using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Role.Response;
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
        private readonly IRoleRepository _repository;
        public RoleService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IRoleRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _repository = repository;
        }

        public async Task<ICollection<GetRoleResponse>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Role> GetRoleByIdAsync(Guid id)
        {
            return await _repository.GetRoleByIdAsync(id);
        }

        public async Task<Role> GetRoleByRoleNameAsync(RoleName roleName)
        {
            return await _repository.GetRoleByRoleNameAsync(roleName);
        }
    }
}
