using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}