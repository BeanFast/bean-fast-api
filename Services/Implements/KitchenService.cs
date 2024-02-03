using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implements;

public class KitchenService : BaseService<Kitchen>, IKitchenService
{
    public KitchenService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IPaginable<Kitchen>> GetKitchenPage(PaginationRequest paginationRequest)
    {
        return await _repository.GetPageAsync(
            paginationRequest: paginationRequest
        );
    }
}