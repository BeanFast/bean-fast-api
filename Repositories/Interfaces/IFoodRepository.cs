using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IFoodRepository : IGenericRepository<Food>
    {
        Task<ICollection<GetFoodResponse>> GetAllFoodsAsync(string? userRole, FoodFilterRequest filterRequest);
        Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest,
            PaginationRequest request);
        Task<Food?> GetByIdAsync(Guid id);
        Task<IPaginable<Food>> GetBestSellerFoodsPageAsync(GetBestSellerFoodsRequest request);
    }
}
