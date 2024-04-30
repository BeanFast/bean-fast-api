using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using DataTransferObjects.Models.Category.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Services.Interfaces
{
    public interface ICategoryService : IBaseService
    {
        public Task<ICollection<Category>> GetAll(string? role);

        public Task CreateCategory(CreateCategoryRequest category, User user);

        public Task<Category?> GetById(Guid id);

        public Task<Category?> GetById(Guid id, int status);
        Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest category, User user);

        Task DeleteCategoryAsync(Guid id, User user);
        Task<ICollection<GetTopSellerCategoryResponse>> GetTopSellerCategory(int topCount);
    }
}
