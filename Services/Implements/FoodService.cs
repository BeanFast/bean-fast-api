using System;
using System.Linq.Expressions;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private readonly IGenericRepository<Food> _foodRepository;
        private readonly AppSettings _appSettings;
        private readonly ICloudStorageService _cloudStorageService;

        private readonly ICategoryService _categoryService;

        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper,
            ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings, ICategoryService categoryService) : base(unitOfWork, mapper)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();
            _cloudStorageService = cloudStorageService;
            _categoryService = categoryService;
            _appSettings = appSettings.Value;
        }

        public async Task<ICollection<Food>> GetAllAsync()
        {
            return await _foodRepository.GetListAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE,
                include: f => f.Include(f => f.Category!));
        }

        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(PaginationRequest request)
        {
            Console.WriteLine(request);
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            IPaginable<GetFoodResponse> page = await _foodRepository.GetPageAsync(
                status: Utilities.Enums.BaseEntityStatus.ACTIVE, paginationRequest: request, selector: selector,
                orderBy: orderBy);
            return page;
        }

        public async Task<GetFoodResponse> GetFoodResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetFoodResponse>(await GetByIdAsync(id));
        }

        public async Task<Food> GetByIdAsync(Guid id)
        {
            Expression<Func<Food, bool>> filter = (food) => food.Id == id;
            var food = await _foodRepository.FirstOrDefaultAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE,
                predicate: filter, include: queryable => queryable.Include(f => f.Category!));
            if (food is null) throw new EntityNotFoundException(MessageConstants.Food.FoodNotFound(id));
            return food;
        }
        public async Task<Food> GetByIdAsync(Guid id, BaseEntityStatus status)
        {
            Expression<Func<Food, bool>> filter = (food) => food.Id == id;
            var food = await _foodRepository.FirstOrDefaultAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE,
                predicate: filter, include: f => f.Include(f => f.Category!));
            if (food is null) throw new EntityNotFoundException(MessageConstants.Food.FoodNotFound(id));
            return food;
        }
        
        public async Task CreateFoodAsync(CreateFoodRequest request)
        {
            Console.WriteLine(request);
            var foodId = Guid.NewGuid();
            string imagePath = await _cloudStorageService.UploadFileAsync(foodId,
                _appSettings.Firebase.FolderNames.Food, request.Image.ContentType, request.Image);
            var foodEntity = _mapper.Map<Food>(request);
            var category = await _categoryService.GetById(request.CategoryId);
            foodEntity.Id = foodId;
            foodEntity.Status = (int)BaseEntityStatus.ACTIVE;
            foodEntity.ImagePath = imagePath;
            if (request.Combos is not null && request.Combos.Count > 0)
            {
                foodEntity.IsCombo = true;
                foreach (var foodEntityCombo in foodEntity.Combos!)
                {
                    await GetByIdAsync(foodEntityCombo.FoodId);
                    foodEntityCombo.MasterFoodId = foodId;
                    foodEntityCombo.Id = Guid.NewGuid();
                    foodEntityCombo.Status = (int)BaseEntityStatus.ACTIVE;
                    foodEntityCombo.Code = "123232";
                }
            }

            await _foodRepository.InsertAsync(foodEntity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var food = await GetByIdAsync(guid);
            _repository.DeleteAsync(food);
            
        }
    }
}