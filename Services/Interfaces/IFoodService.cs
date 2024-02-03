using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using DataTransferObjects.Models.Food.Request;

namespace Services.Interfaces
{
    public interface IFoodService
    {
        Task<ICollection<Food>>  GetAllAsync();
        Task<IPaginable<GetFoodResponse>> GetPageAsync(PaginationRequest request);
        Task<GetFoodResponse> GetByIdAsync(Guid id);
        Task CreateFoodAsync(CreateFoodRequest request);
    }
}
