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
using Utilities.Statuses;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<ICollection<GetTopSellerCategoryResponse>> GetTopSellerCategory(int topCount)
        {
            Func<IQueryable<Category>, IIncludableQueryable<Category, object>> include;
            List<Expression<Func<Category, bool>>> filters = new List<Expression<Func<Category, bool>>>()
            {
                c => c.Foods!.Any(f => f.OrderDetails!.Count > 0 && f.OrderDetails!.Any(od => od.Order!.Status == OrderStatus.Completed))
            };
            include = i => i.Include(c => c.Foods!.Where(f => f.OrderDetails!.Any(od => od.Order!.Status == OrderStatus.Completed)))
                .ThenInclude(f => f.OrderDetails!.Where(od => od.Order!.Status == OrderStatus.Completed))
                .ThenInclude(od => od.Order!);
            
            var categoryList = await _repository.GetListAsync(include: include, filters: filters);
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

            // Calculate the total after rounding
            double totalAfterRounding = roundedData.Sum(c => c.TotalSold);

            // Adjust the last category to ensure the total remains 100%
            if (totalAfterRounding != 100)
            {
                roundedData.Last().TotalSold += 100 - totalAfterRounding;
            }

            return roundedData;
        }
    }
}