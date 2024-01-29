using System;
using System.Linq.Expressions;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Core.Pagination;
using DataTransferObjects.Models.Food.Response;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private IGenericRepository<Food> _foodRepository;
        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();
        }

        public async Task<ICollection<Food>> GetAllAsync()
        {
            return await _foodRepository.GetListAsync(include: f => f.Include(f => f.Category!));
        }

        public async Task<IPaginable<GetFoodResponse>> GetPageAsync(PaginationRequest request)
        {
            Expression<Func<Food, GetFoodResponse>> selector = (f => new GetFoodResponse()
            {
                Code = f.Code,
                Name = f.Name,
                Price = f.Price,
                Discription = f.Discription,
                IsCombo = f.IsCombo,
                ImagePath = f.ImagePath,
                Category = _mapper.Map<GetFoodResponse.CategoryOfFood>(f.Category)
            });
            Func<IQueryable<Food>, IOrderedQueryable<Food>> orderBy = o => o.OrderBy(f => f.Name);
            IPaginable<GetFoodResponse> page =  await _foodRepository.GetPageAsync(paginationRequest: request, selector: selector, orderBy: orderBy);
            return page;
        }

        public async Task<GetFoodResponse> GetByIdAsync(Guid id)
        {
            Expression<Func<Food, bool>> filter = (food) => food.Id == id;
            var food = await _foodRepository.FirstOrDefaultAsync(predicate:filter, include: f => f.Include(f => f.Category!));
            return _mapper.Map<GetFoodResponse>(food);
        }

    }
}
