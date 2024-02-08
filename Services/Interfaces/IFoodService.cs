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
        Task<ICollection<Food>>  GetAllAsync(string? userRole, FoodFilterRequest filterRequest);
        Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest, PaginationRequest request);
        Task<GetFoodResponse> GetFoodResponseByIdAsync(Guid id);
        Task<Food> GetByIdAsync(Guid id);
        Task CreateFoodAsync(CreateFoodRequest request);

        Task UpdateFoodAsync(Guid foodId, UpdateFoodRequest request);

        Task DeleteAsync(Guid guid);
    }
}
