﻿using BusinessObjects.Models;
using DataTransferObjects.Models.Session.Request;
using DataTransferObjects.Models.Session.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISessionService
    {
        Task<ICollection<GetSessionResponse>> GetAllAsync(string? userRole, SessionFilterRequest filterRequest);
        Task<GetSessionResponse> GetSessionResponseByIdAsync(Guid id);
        Task<Session> GetByIdAsync(Guid id);
        Task CreateSessionAsync(CreateSessionRequest request);
        Task UpdateSessionAsync(Guid sessionId, UpdateSessionRequest request);
        Task DeleteAsync(Guid guid);
    }
}
