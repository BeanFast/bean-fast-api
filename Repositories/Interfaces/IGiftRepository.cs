using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Gift.Request;
using DataTransferObjects.Models.Gift.Response;
using Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IGiftRepository : IGenericRepository<Gift>
    {
        Task<Gift> GetGiftByIdAsync(Guid id, int status);
        Task<Gift> GetGiftByIdAsync(Guid id);
        Task<IPaginable<GetGiftResponse>> GetGiftPageAsync(PaginationRequest paginationRequest, GiftFilterRequest filterRequest);
    }
}
