using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Services.Interfaces;

public interface IKitchenService
{
    public Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole);

    public Task CreateKitchenAsync(CreateKitchenRequest request);

    public Task<Kitchen> GetByIdAsync(int status, Guid id);

    public Task<Kitchen> GetByIdAsync(Guid id);


    public Task DeleteKitchenAsync(Guid id);
}