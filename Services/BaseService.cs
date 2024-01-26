using AutoMapper;
using BusinessObjects;
using Repositories.Interfaces;

namespace Services
{
    public class BaseService<T> where T : class
    {
        protected IUnitOfWork<BeanFastContext>? _unitOfWork;
        protected IMapper _mapper;

        public BaseService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //protected ILogger<T> _logger;
        //protected IHttpContextAccessor _httpContextAccessor;

    }
}
