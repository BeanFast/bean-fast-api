using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Utilities.Settings;

namespace Services
{
    public class BaseService<T> where T : BaseEntity
    {
        protected IUnitOfWork<BeanFastContext>? _unitOfWork;
        protected IMapper _mapper;
        protected IGenericRepository<T> _repository;
        protected readonly AppSettings _appSettings;
        public BaseService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _repository = _unitOfWork.GetRepository<T>();
        }


    }
}
