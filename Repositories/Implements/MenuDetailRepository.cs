using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
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
    public class MenuDetailRepository : GenericRepository<MenuDetail>, IMenuDetailRepository
    {
        public MenuDetailRepository(BeanFastContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<MenuDetail> GetByIdAsync(Guid id)
        {
            List<Expression<Func<MenuDetail, bool>>> filters = new()
            {
                (menuDetail) => menuDetail.Id == id && menuDetail.Status != BaseEntityStatus.Deleted
            };
            var menuDetail = await FirstOrDefaultAsync(filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.MenuDetailMessageConstrant.MenuDetailNotFound(id));
            return menuDetail;
        }
    }
}
