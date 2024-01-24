using BusinessObjects;
using Pos_System.Repository.Interfaces;

namespace Services
{
    public class BaseService<T> where T : class
    {
        protected IUnitOfWork<BeanFastContext>? UnitOfWork;
        //protected ILogger<T> _logger;
        //protected IMapper _mapper;
        //protected IHttpContextAccessor _httpContextAccessor;
    }
}
