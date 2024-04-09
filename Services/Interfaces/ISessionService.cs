﻿using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
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
        Task<ICollection<GetDelivererResponse>> GetAvailableDelivererInSessionDeliveryTime(Guid sessionId);
        Task CreateSessionAsync(CreateSessionRequest request);
        Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request);
        Task DeleteAsync(Guid guid);
    }
}
