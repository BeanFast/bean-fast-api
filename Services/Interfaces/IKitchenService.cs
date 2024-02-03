using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;

namespace Services.Interfaces;

public interface IKitchenService
{
    public Task<IPaginable<Kitchen>> GetKitchenPage(PaginationRequest paginationRequest);
}