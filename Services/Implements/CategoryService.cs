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
        public CategoryService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService) : base(unitOfWork, mapper, appSettings)
        {
            _repository = unitOfWork.GetRepository<Category>();
            _cloudStorageService = cloudStorageService;
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

        public async Task CreateCategory(CreateCategoryRequest category, User user)
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
                throw new InvalidRequestException(MessageConstants.CategoryMessageConstrant.CategoryNameExisted);
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(categoryEntity.Id, _appSettings.Firebase.FolderNames.Category, category.Image);
            categoryEntity.ImagePath = imagePath;

            await _repository.InsertAsync(categoryEntity, user);
            return;
        }

        public async Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest category, User user)
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
                throw new InvalidRequestException(MessageConstants.CategoryMessageConstrant.CategoryNameExisted);
            }
            await _cloudStorageService.DeleteFileAsync(id, _appSettings.Firebase.FolderNames.Category);
            var imagePath = await _cloudStorageService.UploadFileAsync(id, _appSettings.Firebase.FolderNames.Category, category.Image);
            categoryEntity.ImagePath = imagePath;
            await _repository.UpdateAsync(categoryEntity, user);
            //await _repository.UpdateAsync()
        }

        public async Task DeleteCategoryAsync(Guid id, User user)
        {
            var category = await GetById(id);
            await _repository.DeleteAsync(category!, user);
        }
    }
}