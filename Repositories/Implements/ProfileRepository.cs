using AutoMapper;
using BusinessObjects;
using Repositories.Interfaces;
using Profile = BusinessObjects.Models.Profile;

namespace Repositories.Implements;

public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
{
    public ProfileRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}