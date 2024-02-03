using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;

namespace Services.Implements
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        public CategoryService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _categoryRepository = unitOfWork.GetRepository<Category>();
        }

        public Task<ICollection<Category>> GetAll(string? role)
        {
            if(role is not null && role == RoleName.ADMIN.ToString())
            {
                return _categoryRepository.GetListAsync();
            }
            return _categoryRepository.GetListAsync(BaseEntityStatus.ACTIVE);
        }

        public async Task<Category> GetById(Guid id)
        {
            var category =  await _categoryRepository.FirstOrDefaultAsync(predicate: c => c.Id == id);
            if(category is null)
            {
                throw new EntityNotFoundException(MessageContants.Category.CategoryNotFound);
            }
            return category!;
        }

        public async Task CreateCategory(CreateCategoryRequest category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            categoryEntity.Id = Guid.NewGuid();
            var checkExistList = await _categoryRepository.GetListAsync(predicate: c => c.Name == category.Name || c.Code == category.Code);
            if (checkExistList.Count > 0)
            {
                throw new DataExistedException(MessageContants.Category.CategoryCodeOrNameExisted);
            }
            await _categoryRepository.InsertAsync(categoryEntity);
            return;
        }

    }
}
