using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Kitchen.Request;
using DataTransferObjects.Models.Kitchen.Response;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Services.Interfaces;

public interface IKitchenService : IBaseService
{
    Task<IPaginable<GetKitchenResponse>> GetKitchenPageAsync(PaginationRequest paginationRequest, KitchenFilterRequest filterRequest, string? userRole);

    Task CreateKitchenAsync(CreateKitchenRequest request, User user);

    Task<Kitchen> GetByIdAsync(int status, Guid id);
    Task<Kitchen?> GetByManagerId(Guid managerId);
    Task<Kitchen> GetByIdAsync(Guid id);

    Task UpdateKitchenAsync(Guid id, UpdateKitchentRequest request, User user);

    Task DeleteKitchenAsync(Guid id, User user);
    Task<int> CountSchoolByKitchenIdAsync(Guid kitchentId);
    Task<ICollection<GetKitchenResponse>> GetAllAsync(string? userRole, KitchenFilterRequest filterRequest);
}