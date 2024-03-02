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
using Utilities.Statuses;
using static Azure.Core.HttpHeader;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private readonly IGenericRepository<Food> _foodRepository;


        private readonly IGenericRepository<Combo> _comboRepository;


        private readonly AppSettings _appSettings;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ICategoryService _categoryService;

        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper,
            ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings,
            ICategoryService categoryService) : base(unitOfWork, mapper)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();

            _comboRepository = unitOfWork.GetRepository<Combo>();


            _cloudStorageService = cloudStorageService;
            _categoryService = categoryService;
            _appSettings = appSettings.Value;
        }

        private List<Expression<Func<Food, bool>>> GetFilterFromFilterRequest(FoodFilterRequest filterRequest)
        {
            List<Expression<Func<Food, bool>>> filters = new();

            if (filterRequest.CategoryId != null && filterRequest.CategoryId != Guid.Empty)
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
        public async Task<ICollection<GetFoodResponse>> GetAllAsync(string? userRole, FoodFilterRequest filterRequest)
        {
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));

            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include = (f) => f.Include(f => f.Category!);
            var filters = GetFilterFromFilterRequest(filterRequest);
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await _foodRepository.GetListAsync(include: include, filters: filters, selector: selector);
            }

            return await _foodRepository.GetListAsync(BaseEntityStatus.Active, include: include, filters: filters, selector: selector);
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
                    status: BaseEntityStatus.Active, paginationRequest: request, selector: selector,
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
            var food = await _foodRepository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(f => f.Category!).Include(f => f.Combos))
                ?? throw new EntityNotFoundException(MessageConstants.Food.FoodNotFound(id));
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
                food = await _foodRepository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: f => f.Include(f => f.Category!).Include(f => f.Combos!));
            }


            if (food is null) throw new EntityNotFoundException(MessageConstants.Food.FoodNotFound(id));
            return food;
        }

        public async Task CreateFoodAsync(CreateFoodRequest request)
        {
            Console.WriteLine(request);
            var masterFoodId = Guid.NewGuid();
            string imagePath = await _cloudStorageService.UploadFileAsync(masterFoodId,
                _appSettings.Firebase.FolderNames.Food, request.Image.ContentType, request.Image);
            var foodEntity = _mapper.Map<Food>(request);
            var category = await _categoryService.GetById(request.CategoryId);
            foodEntity.Id = masterFoodId;
            foodEntity.Status = BaseEntityStatus.Active;
            foodEntity.ImagePath = imagePath;

                var comboEntityList = new List<Combo>();
            if (request.Combos is not null && request.Combos.Count > 0)
            {
                foodEntity.IsCombo = true;

                foreach (var combo in request.Combos!)
                {
                    var comboEntity = new Combo 
                    { 
                        Id = Guid.NewGuid(),
                        MasterFoodId = masterFoodId,
                        FoodId = combo.FoodId,
                        Quantity = combo.Quantity,
                        Code = "123123123",
                        Status = 1
                    };
                    comboEntityList.Add(comboEntity);
                    //await Console.Out.WriteLineAsync(comboEntity.FoodId.ToString());
                    //await GetByIdAsync(comboEntity.FoodId);
                    //comboEntity.MasterFoodId = masterFoodId;
                    //comboEntity.Id = Guid.NewGuid();
                    //comboEntity.Status = BaseEntityStatus.Active;
                    //comboEntity.Code = "123232";

                }
                //foodEntity.Combos = comboEntityList;
            }
            //var comboEntityList = foodEntity.Combos;
            foodEntity.Combos!.Clear();
            await Console.Out.WriteLineAsync(foodEntity.ToString());
            await _foodRepository.InsertAsync(foodEntity);
            await _comboRepository.InsertRangeAsync(comboEntityList);
        }

        public async Task UpdateFoodAsync(Guid foodId, UpdateFoodRequest request)
        {
            var foodEntity = await GetByIdAsync(foodId);
            var category = await _categoryService.GetById(foodEntity.CategoryId, BaseEntityStatus.Active);
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
                var comboEntities =  new List<Combo>();
                foreach (var combo in request.Combos!)
                {
                    await GetByIdAsync(combo.FoodId);
                    var comboEntity = new Combo
                    {
                        MasterFoodId = foodId,
                        FoodId = combo.FoodId,
                        Quantity = combo.Quantity,
                        Code = "Combo_xxx"
                    };
                    comboEntities.Add(comboEntity);
                    await Console.Out.WriteLineAsync();
                }
                foodEntity.Combos!.Clear();
                foodEntity.Combos.ToList().AddRange(comboEntities);

            }
            await _foodRepository.UpdateAsync(foodEntity);
        }

        public async Task DeleteAsync(Guid guid)
        {
            var food = await GetByIdAsync(guid);
            await _repository.DeleteAsync(food);
        }

    }
}