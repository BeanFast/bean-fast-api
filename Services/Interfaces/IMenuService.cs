using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Response;

namespace Services.Interfaces;

public interface IMenuService
{
    Task<IPaginable<GetMenuListResponse>> GetMenuPage(PaginationRequest request);
}