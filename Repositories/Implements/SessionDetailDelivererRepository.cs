using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System.Linq.Expressions;
using Utilities.Statuses;

namespace Repositories.Implements;

public class SessionDetailDelivererRepository : GenericRepository<SessionDetailDeliverer>, ISessionDetailDelivererRepository
{
    public SessionDetailDelivererRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId)
    {
        var filters = new List<Expression<Func<SessionDetailDeliverer, bool>>>
        {
            sdd => sdd.SessionDetailId == sessionDetailId && sdd.Status != BaseEntityStatus.Deleted
        };
        return await GetListAsync(filters: filters) ;
    }
}