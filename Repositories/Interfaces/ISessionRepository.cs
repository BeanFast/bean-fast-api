using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<Session?> GetByIdForDelete(Guid id);
        Task<ICollection<Session>> GetOverlappedDeliveryTimeSessions(DateTime deliveryStartTime, DateTime deliveryEndTime);
        Task<Session?> GetSessionByMenuDetailIdAndProfileIdAndSessionIdAsync(Guid menuDetailId, Guid profileId, Guid sessionId);
        Task<GetSessionForDeliveryResponse> GetSessionForDeliveryResponseByIdAsync(Guid id, SessionFilterRequest request, string? userRole);
        Task<ICollection<GetSessionForDeliveryResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest);
        Task<Session> GetByIdAsync(Guid id);
        Task<Session> GetBySessionDetailIdAsync(Guid sesionDetailId);
    }
}
