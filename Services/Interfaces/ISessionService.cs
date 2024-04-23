using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISessionService : IBaseService
    {
        Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest);
        Task<GetSessionForDeliveryResponse> GetSessionResponseByIdAsync(Guid id);
        Task<Session> GetByIdAsync(Guid id);
        Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id, SessionFilterRequest request, string? userRole);
        Task<ICollection<GetDelivererResponse>> GetAvailableDelivererInSessionDeliveryTime(Guid sessionDetailId);
        Task CreateSessionAsync(CreateSessionRequest request, User user);
        Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request, User user);
        Task DeleteAsync(Guid guid, User user);
        Task UpdateOrdersStatusAutoAsync();
        Task UpdateSessionDetailByIdAsync(Guid id, UpdateSessionDetailRequest request);
    }
}
