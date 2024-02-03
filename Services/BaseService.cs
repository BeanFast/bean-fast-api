using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;

namespace Services
{
    public class BaseService<T> where T : BaseEntity
    {
        protected IUnitOfWork<BeanFastContext>? _unitOfWork;
        protected IMapper _mapper;
        protected IGenericRepository<T> _repository;
        public BaseService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<T>();
        }

        //protected ILogger<T> _logger;
        //protected IHttpContextAccessor _httpContextAccessor;

    }
}
