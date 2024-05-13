using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category?> GetById(Guid id, int status);
        Task<ICollection<Category>> GetByName(string categoryName);
        Task<ICollection<Category>> GetCategoriesForDashboard();
    }
}
