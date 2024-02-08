using System;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
            ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings,
            ICategoryService categoryService) : base(unitOfWork, mapper)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();
            _cloudStorageService = cloudStorageService;
            _categoryService = categoryService;
            _appSettings = appSettings.Value;
        }

        private List<Expression<Func<Food, bool>>> GetFilterFromFilterRequest(FoodFilterRequest filterRequest)
        {
            List<Expression<Func<Food, bool>>> filters = new();

            if (filterRequest.CategoryId != null)
            {
                filters.Add((f) => f.CategoryId == filterRequest.CategoryId);
            }

            if (filterRequest.Code != null)
            {
                filters.Add(f => f.Code == filterRequest.Code);
            }

            if (filterRequest.Name is { Length: > 0 })
            {
                filters.Add(f => f.Name.ToLower().Contains(filterRequest.Name.ToLower()));
            }

            if (filterRequest.MinPrice > 0)
            {
                filters.Add(f => f.Price >= filterRequest.MinPrice);
            }
            if (filterRequest.MaxPrice > 0)
            {
                filters.Add(f => f.Price <= filterRequest.MaxPrice);
            }

            return filters;
        }
        public async Task<ICollection<Food>> GetAllAsync(string? userRole, FoodFilterRequest filterRequest)
        {
            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include = (f) => f.Include(f => f.Category!);
            var filters = GetFilterFromFilterRequest(filterRequest);
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await _foodRepository.GetListAsync(include: include, filters: filters);
            }

            return await _foodRepository.GetListAsync(BaseEntityStatus.ACTIVE, include: include, filters: filters);
        }

        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest,
            PaginationRequest request)
        {
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            IPaginable<GetFoodResponse>? page = null;
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                page = await _foodRepository.GetPageAsync(
                    paginationRequest: request, selector: selector,
                    orderBy: orderBy);
            }
            else
            {
                page = await _foodRepository.GetPageAsync(
                    status: Utilities.Enums.BaseEntityStatus.ACTIVE, paginationRequest: request, selector: selector,
                    orderBy: orderBy);
            }

            return page;
        }

        public async Task<GetFoodResponse> GetFoodResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetFoodResponse>(await GetByIdAsync(id));
        }

        public async Task<Food> GetByIdAsync(Guid id)
        {
            List<Expression<Func<Food, bool>>> filters = new()
            {
                (food) => food.Id == id,
            };
            var food = await _foodRepository.FirstOrDefaultAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE,
                filters: filters, include: queryable => queryable.Include(f => f.Category!).Include(f => f.Combos));
            if (food is null) throw new EntityNotFoundException(MessageConstants.Food.FoodNotFound(id));
            return food;
        }

        public async Task<Food> GetByIdAsync(Guid id, string roleName)
        {
            Food? food = null;
            List<Expression<Func<Food, bool>>> filters = new()
            {
                (food) => food.Id == id,
            };
            if (roleName.Equals(RoleName.ADMIN.ToString()))
            {
                food = await _foodRepository.FirstOrDefaultAsync(
                    filters: filters, include: f => f.Include(f => f.Category!).Include(f => f.Combos!));
            }
            else
            {
                food = await _foodRepository.FirstOrDefaultAsync(status: BaseEntityStatus.ACTIVE,
                filters: filters, include: f => f.Include(f => f.Category!).Include(f => f.Combos!));
            }


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

        public async Task UpdateFoodAsync(Guid foodId, UpdateFoodRequest request)
        {
            var foodEntity = await GetByIdAsync(foodId);
            var category = await _categoryService.GetById(foodEntity.CategoryId, BaseEntityStatus.ACTIVE);
            foodEntity.Price = request.Price;
            foodEntity.Description = request.Description;
            foodEntity.Name = request.Name;
            foodEntity.Category = category;
            if (request.Image != null)
            {
                await _cloudStorageService.DeleteFileAsync(foodId, _appSettings.Firebase.FolderNames.Food);
                string newFoodImageUrl = await _cloudStorageService.UploadFileAsync(foodId, _appSettings.Firebase.FolderNames.Food, request.Image.ContentType, request.Image);
                foodEntity.ImagePath = newFoodImageUrl;
            }
            if (request.Combos is not null && request.Combos.Count > 0)
            {
                foodEntity.IsCombo = true;
                var comboEntity =  new List<Combo>();
                foreach (var combo in request.Combos!)
                {
                    await GetByIdAsync(combo.FoodId);
                    comboEntity.Add(new Combo
                    {
                        MasterFoodId = foodId,
                        FoodId = combo.FoodId,
                        Quantity = combo.Quantity,
                        Code = "Combo_xxx"
                    });
                    foodEntity.Combos = comboEntity;
                }
            }
            await _foodRepository.UpdateAsync(foodEntity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var food = await GetByIdAsync(guid);
            _repository.DeleteAsync(food);
        }
    }
}