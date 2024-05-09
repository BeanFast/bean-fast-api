using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Gift.Request;
using DataTransferObjects.Models.Gift.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGiftService : IBaseService
    {
        Task CreateGiftAsync(CreateGiftRequest request, User user);
        Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest);
        Task<Gift> GetGiftByIdAsync(Guid id, int status);
        Task<Gift> GetGiftByIdAsync(Guid id);
        Task UpdateGiftAsync(Guid id, UpdateGiftRequest request, User user);
        Task UpdateGiftAsync(Gift gift);
        Task DeleteGiftAsync(Guid id, User user);

    }
}
