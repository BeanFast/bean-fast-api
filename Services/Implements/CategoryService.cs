using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {

        private readonly ICloudStorageService _cloudStorageService;
        private readonly AppSettings _appSettings;
        public CategoryService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper)
        {
            _repository = unitOfWork.GetRepository<Category>();
            _cloudStorageService = cloudStorageService;
            _appSettings = appSettings.Value;
        }

        public Task<ICollection<Category>> GetAll(string? role)
        {
            if (role is not null && role == RoleName.ADMIN.ToString())
            {
                return _repository.GetListAsync();
            }

            return _repository.GetListAsync(BaseEntityStatus.Active);
        }

        public async Task<Category?> GetById(Guid id)
        {
            var category = await _repository.FirstOrDefaultAsync(filters: new() { c => c.Id == id });
            if (category is null)
            {
                throw new EntityNotFoundException(MessageConstants.CategoryMessageConstrant.CategoryNotFound);
            }

            return category!;
        }

        public async Task<Category?> GetById(Guid id, int status)
        {
            var category = await _repository.FirstOrDefaultAsync(status, filters: new() { c => c.Id == id });
            if (category is null)
            {
                throw new EntityNotFoundException(MessageConstants.CategoryMessageConstrant.CategoryNotFound);
            }

            return category!;
        }

        public async Task CreateCategory(CreateCategoryRequest category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            categoryEntity.Id = Guid.NewGuid();
            var checkExistList =
                await _repository.GetListAsync(filters: new()
                {
                    c =>
                        c.Name == category.Name
                });
            if (checkExistList.Count > 0)
            {
                throw new DataExistedException(MessageConstants.CategoryMessageConstrant.CategoryNameExisted);
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(categoryEntity.Id, _appSettings.Firebase.FolderNames.Category, category.Image);
            categoryEntity.ImagePath = imagePath;

            await _repository.InsertAsync(categoryEntity);
            return;
        }

        public async Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest category)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            categoryEntity.Id = id;
            var checkExistList =
                await _repository.GetListAsync(filters: new()
                {
                    c =>
                        c.Name == category.Name
                });
            if (checkExistList.Count > 0)
            {
                throw new DataExistedException(MessageConstants.CategoryMessageConstrant.CategoryNameExisted);
            }
            await _cloudStorageService.DeleteFileAsync(id, _appSettings.Firebase.FolderNames.Category);
            var imagePath = await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.Category, category.Image);
            categoryEntity.ImagePath = imagePath;
            await _repository.UpdateAsync(categoryEntity);
            //await _repository.UpdateAsync()
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await GetById(id);
            await _repository.DeleteAsync(category!);
        }
    }
}