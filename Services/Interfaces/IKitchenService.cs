using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;

namespace Services.Interfaces;

public interface IKitchenService
{
    public Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole);

    public Task CreateKitchenAsync(CreateKitchenRequest request);
    
    public Task DeleteKitchenAsync(Guid id);
}