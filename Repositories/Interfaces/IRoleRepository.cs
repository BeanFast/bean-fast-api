using BusinessObjects.Models;
using DataTransferObjects.Models.Role.Response;
using Utilities.Enums;

namespace Repositories.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Task<ICollection<GetRoleResponse>> GetAllAsync();
    Task<Role> GetRoleByIdAsync(Guid id);

    Task<Role> GetRoleByRoleNameAsync(RoleName roleName);
}