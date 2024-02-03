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
using Utilities.Enums;
using Utilities.Settings;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private readonly IGenericRepository<Food> _foodRepository;
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly AppSettings _appSettings;
        private readonly ICloudStorageService _cloudStorageService;
        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, ICloudStorageService cloudStorageService, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();
            _categoryRepository = unitOfWork.GetRepository<Category>();
            _cloudStorageService = cloudStorageService;
            _appSettings = appSettings.Value;
        }
        public async Task<ICollection<Food>> GetAllAsync()
        {
            return await _foodRepository.GetListAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE, include: f => f.Include(f => f.Category!));
        }

        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(PaginationRequest request)
        {
            Console.WriteLine(request);
            Expression<Func<Food, GetFoodResponse>> selector = (f => _mapper.Map<GetFoodResponse>(f));
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            IPaginable<GetFoodResponse> page =  await _foodRepository.GetPageAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE,paginationRequest: request, selector: selector, orderBy: orderBy);
            return page;
        }

        public async Task<GetFoodResponse> GetByIdAsync(Guid id)
        {
            Expression<Func<Food, bool>> filter = (food) => food.Id == id;
            var food = await _foodRepository.FirstOrDefaultAsync(status: Utilities.Enums.BaseEntityStatus.ACTIVE, predicate:filter, include: f => f.Include(f => f.Category!));
            return _mapper.Map<GetFoodResponse>(food);
        }

        public async Task CreateFoodAsync(CreateFoodRequest request)
        {
            var foodId = Guid.NewGuid();
            string imagePath = await _cloudStorageService.UploadFileAsync(foodId, _appSettings.Firebase.FolderNames.Food, request.Image.ContentType, request.Image);
            var foodEntity = _mapper.Map<Food>(request);
            foodEntity.Id = foodId;
            foodEntity.Status = (int) BaseEntityStatus.ACTIVE;
            foodEntity.ImagePath = imagePath;
            await _foodRepository.InsertAsync(foodEntity);
        }

    } 
}
