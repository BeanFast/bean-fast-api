using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.SessionDetail.Request;
using DataTransferObjects.Models.SessionDetail.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ISessionDetailService : IBaseService
    {
        Task<SessionDetail> GetByIdAsync(Guid id);
        Task<GetSessionDetailResponse> GetSessionDetailResponseByIdAsync(Guid id);
        Task<ICollection<GetSessionDetailResponse>> GetSessionDetailsAsync(User user, GetSessionDetailFilterRequest filterReqeuest);
        Task<ICollection<GetSessionDetailResponse>> GetSessionDetailByDelivererIdAsync(User user);
        Task<ICollection<GetSessionDetailResponse>> GetIncommingDeliveringSessionDetailsAsync(User user);
        Task CreateSessionDetailAsync(CreateSessionDetailRequest createSessionDetail);
        Task UpdateSessionDetailByIdAsync(Guid sessionDetailId, UpdateSessionDetailRequest updateSessionDetailRequest, List<Guid> availableDelivererIds);
        //Task<bool> CheckSessionDetailAsync(CheckSessionDetailRequest request, Guid sessionDetailId);
    }
}
