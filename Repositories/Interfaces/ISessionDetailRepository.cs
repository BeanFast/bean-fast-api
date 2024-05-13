using BusinessObjects.Models;
using DataTransferObjects.Models.SessionDetail.Response;

namespace Repositories.Interfaces;

public interface ISessionDetailRepository : IGenericRepository<SessionDetail>
{
    Task<SessionDetail> GetByIdAsync(Guid id);
    Task<ICollection<GetSessionDetailResponse>> GetSessionDetailByDelivererIdAsync(User user);
    Task<ICollection<GetSessionDetailResponse>> GetIncommingDeliveringSessionDetailsAsync(User user);
}