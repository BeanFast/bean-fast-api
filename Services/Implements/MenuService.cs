using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Menu.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implements;

public class MenuService: BaseService<Menu>, IMenuService
{
    private readonly IGenericRepository<Menu> _menuRepository;
    public MenuService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
        _menuRepository = unitOfWork.GetRepository<Menu>();
    }


    public async Task<IPaginable<GetMenuListResponse>> GetMenuPage(PaginationRequest request)
    {
        var menuPage = await _menuRepository.GetPageAsync(
                paginationRequest: request,
                include: i => i.Include(menu => menu.Kitchen),
                selector: menu => _mapper.Map<GetMenuListResponse>(menu)
            );
        return menuPage;
    }
}