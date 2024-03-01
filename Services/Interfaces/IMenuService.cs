using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;

namespace Services.Interfaces;

public interface IMenuService
{
    Task<IPaginable<GetMenuListResponse>> GetMenuPage(PaginationRequest request);

    Task CreateMenuAsync(CreateMenuRequest createMenuRequest);
}