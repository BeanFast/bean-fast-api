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
    public interface IGiftService
    {
        Task CreateGiftAsync(CreateGiftRequest request);
        Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest);
        Task<Gift> GetGiftByIdAsync(int status, Guid id);
        Task<Gift> GetGiftByIdAsync(Guid id);
        Task UpdateGiftAsync(Guid id, UpdateGiftRequest request);
        Task DeleteGiftAsync(Guid id);

    }
}
