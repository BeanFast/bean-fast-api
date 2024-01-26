using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class FoodService : BaseService<Food>, IFoodService
    {
        private IGenericRepository<Food> _foodRepository;
        public FoodService(IUnitOfWork<BeanFastContext> unitOfWork) : base(unitOfWork)
        {
            _foodRepository = unitOfWork.GetRepository<Food>();
        }

        public async Task<ICollection<Food>> GetAllAsync()
        {
            return await _foodRepository.GetListAsync(include: f => f.Include(f => f.Category!));
        }
    }
}
