using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Request;
using DataTransferObjects.Models.Menu.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implements;

public class MenuService: BaseService<Menu>, IMenuService
{
    private readonly IKitchenService _kitchenService;
    public MenuService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IKitchenService kitchenService) : base(unitOfWork, mapper)
    {
        _kitchenService = kitchenService;
    }

    public Task CreateMenuAsync(CreateMenuRequest createMenuRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<IPaginable<GetMenuListResponse>> GetMenuPage(PaginationRequest request)
    {
        var menuPage = await _repository.GetPageAsync(
                paginationRequest: request,
                include: i => i.Include(menu => menu.Kitchen),
                selector: menu => _mapper.Map<GetMenuListResponse>(menu)
            );
        return menuPage;
    }
    public async Task CreateMenuAsync(CreateMenuRequest createMenuRequest, Guid createrId)
    {
        await _kitchenService.GetByIdAsync(Utilities.Enums.BaseEntityStatus.ACTIVE, createMenuRequest.KitchenId);

        var menuEntity = _mapper.Map<Menu>(createMenuRequest);
        menuEntity.CreaterId = createrId;
        menuEntity.UpdaterId = createrId;
        menuEntity.CreateDate = DateTime.UtcNow;
        menuEntity.UpdateDate = DateTime.UtcNow;
        //menuEntity.
        
    }
}