using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Request;
using DataTransferObjects.Models.Food.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.Interfaces;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;
using static Azure.Core.HttpHeader;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private readonly ICloudStorageService _cloudStorageService;
        private readonly ICategoryService _categoryService;
        private readonly IComboService _comboService;

        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper,
            ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings,
            ICategoryService categoryService, IComboService comboService) : base(unitOfWork, mapper, appSettings)
        {

            _cloudStorageService = cloudStorageService;
            _categoryService = categoryService;
            _comboService = comboService;
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

            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include = (f) => f.Include(f => f.Category!);
            var filters = GetFilterFromFilterRequest(filterRequest);
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                return await _repository.GetListAsync<GetFoodResponse>(include: include, filters: filters);
            }

            return await _repository.GetListAsync<GetFoodResponse>(BaseEntityStatus.Active, include: include, filters: filters);
        }

        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(string? userRole, FoodFilterRequest filterRequest,
            PaginationRequest request)
        {
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            IPaginable<GetFoodResponse>? page = null;
            if (RoleName.ADMIN.ToString().Equals(userRole))
            {
                page = await _repository.GetPageAsync<GetFoodResponse>(paginationRequest: request, orderBy: orderBy);
            }
            else
            {
                page = await _repository.GetPageAsync<GetFoodResponse>(
                    status: BaseEntityStatus.Active,
                    paginationRequest: request, orderBy: orderBy);
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

            var food = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(f => f.Category!).Include(f => f.Combos!).Include(f => f.MasterCombos!))
                ?? throw new EntityNotFoundException(MessageConstants.FoodMessageConstrant.FoodNotFound(id));
            return food;
        }
        public async Task<Food> GetByIdForUpdateActionAsync(Guid id)
        {
            List<Expression<Func<Food, bool>>> filters = new()
            {
                (food) => food.Id == id,
            };

            var food = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: queryable => queryable.Include(f => f.Category!).Include(f => f.Combos!).Include(f => f.MasterCombos!).Include(f => f.MenuDetails!).ThenInclude(md => md.Menu!))
                ?? throw new EntityNotFoundException(MessageConstants.FoodMessageConstrant.FoodNotFound(id));
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
                food = await _repository.FirstOrDefaultAsync(
                    filters: filters, include: f => f.Include(f => f.Category!).Include(f => f.Combos!));
            }
            else
            {
                food = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters, include: f => f.Include(f => f.Category!).Include(f => f.Combos!));
            }


            if (food is null) throw new EntityNotFoundException(MessageConstants.FoodMessageConstrant.FoodNotFound(id));
            return food;
        }

        public async Task CreateFoodAsync(CreateFoodRequest request, User user)
        {
            Console.WriteLine(request);
            var masterFoodId = Guid.NewGuid();
            string imagePath = await _cloudStorageService.UploadFileAsync(masterFoodId,
                _appSettings.Firebase.FolderNames.Food, request.Image);
            var foodEntity = _mapper.Map<Food>(request);
            await _categoryService.GetById(request.CategoryId);
            foodEntity.Id = masterFoodId;
            foodEntity.Status = BaseEntityStatus.Active;
            foodEntity.ImagePath = imagePath;
            var foodNumber = await _repository.CountAsync() + 1;
            foodEntity.Code = EntityCodeUtil.GenerateEntityCode(EntityCodeConstrant.FoodCodeConstrant.FoodPrefix, foodNumber);
            var comboEntityList = new List<Combo>();

            if (request.Combos is not null && request.Combos.Count > 0)
            {
                foodEntity.IsCombo = true;

                foreach (var combo in request.Combos!)
                {
                    var comboEntity = new Combo
                    {
                        MasterFoodId = masterFoodId,
                        FoodId = combo.FoodId,
                        Quantity = combo.Quantity,
                    };
                    comboEntityList.Add(comboEntity);

                }
            }
            foodEntity.Combos?.Clear();
            await _repository.InsertAsync(foodEntity, user);
            await _comboService.CreateComboListAsync(comboEntityList, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateFoodAsync(Guid foodId, UpdateFoodRequest request, User user)
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
                string newFoodImageUrl = await _cloudStorageService.UploadFileAsync(foodId, _appSettings.Firebase.FolderNames.Food, request.Image);
                foodEntity.ImagePath = newFoodImageUrl;
            }
            var comboEntities = new List<Combo>();
            if (request.Combos is not null && request.Combos.Count > 0)
            {
                foodEntity.IsCombo = true;
                foreach (var combo in request.Combos!)
                {
                    await GetByIdAsync(combo.FoodId);
                    var comboEntity = new Combo
                    {
                        MasterFoodId = foodId,
                        FoodId = combo.FoodId,
                        Quantity = combo.Quantity
                    };
                    comboEntities.Add(comboEntity);
                }
                foodEntity.Combos?.Clear();
                if (!foodEntity.MasterCombos.IsNullOrEmpty())
                    await _comboService.HardDeleteComboListAsync(foodEntity.MasterCombos!);
                await _comboService.CreateComboListAsync(comboEntities, user);
            }
            await _repository.UpdateAsync(foodEntity, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(Guid guid, User user)
        {
            var food = await GetByIdForUpdateActionAsync(guid);
            // check food is existed in other combos
            var message = string.Empty;
            if (food.Combos != null && food.Combos.Any())
            {
                var comboCodes = food.Combos.Select(c => c.Code).ToList();
                message = $"Món ăn này hiện tại đang nằm trong {(comboCodes.Count == 1 ? "" : "các ")} combo: {string.Join(", ", comboCodes)}, vui lòng xóa chúng trong các combo này.";

            }
            if (food.MenuDetails != null && food.MenuDetails.Any())
            {
                //var menuDetailsCode = food.MenuDetails.
                var menudDetailCodes = food.MenuDetails.Select(md => md.Menu!.Code).Distinct().ToList();
                message += "\n";
                message += $"Món ăn này hiện tại đang nằm trong {(menudDetailCodes.Count == 1 ? "" : "các ")} menu: {string.Join(", ", menudDetailCodes)}, vui lòng xóa chúng trong các menu này.";
            }
            if (message != string.Empty) { throw new InvalidRequestException(message); }
            await _repository.DeleteAsync(food, user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ICollection<GetBestSellerFoodsResponse>> GetBestSellerFoodsAsync(GetBestSellerFoodsRequest request)
        {
            var filters = new List<Expression<Func<Food, bool>>>();
            Func<IQueryable<Food>, IIncludableQueryable<Food, object>> include;
            
            if (request.StartDate != DateTime.MinValue && request.EndDate != DateTime.MinValue)
            {
                include = i => i
                .Include(f => f.OrderDetails!
                    .Where(od => od.Order!.PaymentDate.Date >= request.StartDate.Date && od.Order.PaymentDate.Date <= request.EndDate.Date)
                );
            }
            else
            {
                include = i => i.Include(f => f.OrderDetails!);
            }
            var foodPage = await _repository.GetPageAsync(
                filters: filters,
                include: include,
                orderBy: o => o.OrderByDescending(f => f.OrderDetails!.Count),
                paginationRequest: new PaginationRequest
                {
                    Page = 1,
                    Size = request.Number
                });
            //_mapper.Map<ICollection<GetBestSellerFoodsResponse>>(foodPage.Items);
            return _mapper.Map<ICollection<GetBestSellerFoodsResponse>>(foodPage.Items);
        }
    }
}