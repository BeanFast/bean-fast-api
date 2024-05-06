using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class SessionDetailRepository : GenericRepository<SessionDetail>, ISessionDetailRepository
{
    public SessionDetailRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}