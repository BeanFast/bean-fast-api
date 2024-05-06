using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class SessionDetailDelivererRepository : GenericRepository<SessionDetailDeliverer>, ISessionDetailDelivererRepository
{
    public SessionDetailDelivererRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }
}