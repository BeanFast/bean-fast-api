using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<ICollection<Category>> GetAll(string? role);

        public Task CreateCategory(CreateCategoryRequest category);

        public Task<Category?> GetById(Guid id);
    }
}
