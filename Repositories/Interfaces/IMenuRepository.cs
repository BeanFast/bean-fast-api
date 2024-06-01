using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;

namespace Repositories.Interfaces
{
    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<Menu> GetByIdAsync(Guid id);
        Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string? userRole, MenuFilterRequest menuFilterRequest);
        Task<ICollection<GetMenuResponse>> GetAllAsync(string? userRole, MenuFilterRequest menuFilterRequest);
        Task<GetMenuResponse> GetGetMenuResponseByIdAsync(Guid id);
        Task<ICollection<GetMenuResponse>> GetAllAsync(User manager, MenuFilterRequest menuFilterRequest);
        //Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, User manager, MenuFilterRequest menuFilterRequest);
    }
}
