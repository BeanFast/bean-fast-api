using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Repositories.Implements;

public class MenuRepository : GenericRepository<Menu>, IMenuRepository
{
    public MenuRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
    {

    }
}