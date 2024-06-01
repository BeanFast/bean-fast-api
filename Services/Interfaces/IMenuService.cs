using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;

namespace Services.Interfaces;

public interface IMenuService : IBaseService
{
    Task<IPaginable<GetMenuResponse>> GetPageAsync(PaginationRequest request, string? userRole, MenuFilterRequest menuFilterRequest);
    Task<ICollection<GetMenuResponse>> GetAllAsync(string? userRole, MenuFilterRequest menuFilterRequest);

    Task<Menu> GetByIdAsync(Guid id);
    Task<GetMenuResponse> GetGetMenuResponseByIdAsync(Guid id);
    Task<ICollection<GetMenuResponse>> GetAllAsync(User manager, MenuFilterRequest menuFilterRequest);
    Task CreateMenuAsync(CreateMenuRequest createMenuRequest, User creator);
    Task DeleteMenuAsync(Guid id);
    Task UpdateMenuAsync(UpdateMenuRequest request, Guid guid, User updater);
}