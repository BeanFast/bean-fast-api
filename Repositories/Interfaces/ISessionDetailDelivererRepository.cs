using BusinessObjects.Models;

namespace Repositories.Interfaces;

public interface ISessionDetailDelivererRepository : IGenericRepository<SessionDetailDeliverer>
{
    Task<ICollection<SessionDetailDeliverer>> GetBySessionDetailId(Guid sessionDetailId);
}