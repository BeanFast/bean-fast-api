using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        public CategoryService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _categoryRepository = unitOfWork.GetRepository<Category>();
        }

        public Task<ICollection<Category>> GetAll()
        {
            return new List<Category>();
        }
    }
}
