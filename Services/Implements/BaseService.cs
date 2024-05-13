using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Settings;

namespace Services.Implements
{
    public class BaseService<T> : IBaseService where T : BaseEntity
    {
        protected IUnitOfWork<BeanFastContext> _unitOfWork;
        protected IMapper _mapper;
        //protected IGenericRepository<T> _repository;
        protected AppSettings _appSettings;
        public BaseService(
            IUnitOfWork<BeanFastContext> unitOfWork,
            IMapper mapper, 
            IOptions<AppSettings> appSettings)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            //_repository = _unitOfWork.GetRepository<T>();
        }

        public async Task<int> CountAsync()
        {
            return await _unitOfWork.Context.Set<T>().CountAsync();
        }
    }
}
