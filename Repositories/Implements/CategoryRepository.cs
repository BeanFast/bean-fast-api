using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Statuses;

namespace Repositories.Implements
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<Category?> GetById(Guid id, int status)
        {
            var category = await FirstOrDefaultAsync(filters: new() { c => c.Id == id && c.Status == status });
            if (category is null)
            {
                throw new EntityNotFoundException(MessageConstants.CategoryMessageConstrant.CategoryNotFound);
            }

            return category!;
        }

        public async Task<ICollection<Category>> GetByName(string categoryName)
        {
            return await  GetListAsync(filters: new()
                {
                    c =>
                        c.Name == categoryName
                });
        }

        public async Task<ICollection<Category>> GetCategoriesForDashboard(User user)
        {
            Func<IQueryable<Category>, IIncludableQueryable<Category, object>> include;
            List<Expression<Func<Category, bool>>> filters = new List<Expression<Func<Category, bool>>>()
            {
                c => c.Foods!.Any(f => f.OrderDetails!.Count > 0 && f.OrderDetails!.Any(od => od.Order!.Status == OrderStatus.Completed && od.Order.SessionDetail!.Location!.School!.KitchenId == user.Kitchen.Id))
            };
            include = i => i.Include(c => c.Foods!.Where(f => f.OrderDetails!.Any(od => od.Order!.Status == OrderStatus.Completed)))
                .ThenInclude(f => f.OrderDetails!.Where(od => od.Order!.Status == OrderStatus.Completed))
                .ThenInclude(od => od.Order!);

            return await GetListAsync(include: include, filters: filters);
        }
    }
}
