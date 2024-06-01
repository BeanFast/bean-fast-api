using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Category.Request;
using DataTransferObjects.Models.Category.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System.Linq.Expressions;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;

namespace Services.Implements
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly ICloudStorageService _cloudStorageService;

        public CategoryService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, ICloudStorageService cloudStorageService, ICategoryRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            //_repository = unitOfWork.GetRepository<Category>();
            _cloudStorageService = cloudStorageService;
            _repository = repository;
        }

        public Task<ICollection<Category>> GetAll(string? role)
        {
            //if (role is not null && role == RoleName.ADMIN.ToString())
            //{
            //    return _repository.GetListAsync();
            //}
            //return _repository.GetListAsync(filters: new List<Expression<Func<Category, bool>>>
            //{
            //    c => c.Status != BaseEntityStatus.Deleted
            //});
            return _repository.GetListAsync();
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
            return await _repository.GetById(id, status);
        }

        public async Task CreateCategory(CreateCategoryRequest category, User user)
        {
            var categoryEntity = _mapper.Map<Category>(category);
            categoryEntity.Id = Guid.NewGuid();
            var checkExistList =
                await _repository.GetByName(category.Name);
            if (checkExistList.Count > 0)
            {
                throw new InvalidRequestException(MessageConstants.CategoryMessageConstrant.CategoryNameExisted);
            }
            var imagePath = await _cloudStorageService.UploadFileAsync(categoryEntity.Id, _appSettings.Firebase.FolderNames.Category, category.Image);
            categoryEntity.ImagePath = imagePath;

            await _repository.InsertAsync(categoryEntity, user);
            await _unitOfWork.CommitAsync();
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
        public async Task<ICollection<GetTopSellerCategoryResponse>> GetTopSellerCategory(int topCount, User user)
        {
            var categoryList = await _repository.GetCategoriesForDashboard(user);
            var totalSoldCount = categoryList.Sum(c => c.Foods!.Sum(f => f.OrderDetails!.Sum(od => od.Quantity)));
            var data = categoryList.GroupBy(c => c.Name)
                .Select(c =>
                {
                    double totalSold = c.Sum(c => c.Foods!.Sum(f => f.OrderDetails!.Sum(od => od.Quantity)));
                    return new GetTopSellerCategoryResponse
                    {
                        Category = c.Key,
                        TotalSold = totalSold / totalSoldCount * 100
                    };
                })
                .OrderByDescending(c => c.TotalSold);
            var topCategories = data.Take(topCount).ToList();
            if (data.Count() > topCount)
            {
                topCategories.Add(new GetTopSellerCategoryResponse { Category = "Others", TotalSold = data.Skip(topCount).Sum(x => x.TotalSold)});
            }
            // rounded percentage to 1 decimal place
            var roundedData = topCategories.Select(c => new GetTopSellerCategoryResponse
            {
                Category = c.Category,
                TotalSold = Math.Round(c.TotalSold, 1)
            }).ToList();
            if(roundedData.Count == 0) return roundedData;
            // Calculate the total after rounding
            double totalAfterRounding = roundedData.Sum(c => c.TotalSold);

            // Adjust the last category to ensure the total remains 100%
            if (totalAfterRounding != 100)
            {
                decimal extraPercent = 100 - (decimal)totalAfterRounding;
                roundedData.Last().TotalSold += (double) extraPercent;
            }

            return roundedData;
        }
    }
}