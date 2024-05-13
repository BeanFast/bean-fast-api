using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Role.Response;
using Repositories.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
    public async Task<ICollection<GetRoleResponse>> GetAllAsync()
    {
        var result = await GetListAsync<GetRoleResponse>(filters: new()
            {
                r => r.Status != BaseEntityStatus.Deleted
            });
        return result;
    }

    public async Task<Role> GetRoleByIdAsync(Guid id)
    {
        var result = await FirstOrDefaultAsync(filters: new()
            {
                r => r.Id == id && r.Status != BaseEntityStatus.Deleted
            });
        if (result == null) { throw new EntityNotFoundException(message: MessageConstants.AuthorizationMessageConstrant.RoleNotFound); }
        return result;
    }

    public async Task<Role> GetRoleByRoleNameAsync(RoleName roleName)
    {
        var role = await FirstOrDefaultAsync(filters: new()
            {
                r => r.EnglishName == roleName.ToString()
            });
        return role!;
    }
}