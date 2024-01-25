using BusinessObjects;
using Repositories.Interfaces;

namespace Services
{
    public class BaseService<T> where T : class
    {
        protected IUnitOfWork<BeanFastContext>? UnitOfWork;

        public BaseService(IUnitOfWork<BeanFastContext>? unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        //protected ILogger<T> _logger;
        //protected IMapper _mapper;
        //protected IHttpContextAccessor _httpContextAccessor;

    }
}
