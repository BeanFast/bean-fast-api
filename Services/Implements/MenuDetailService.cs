using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Exceptions;
using Utilities.Settings;
using Utilities.Statuses;

namespace Services.Implements
{
    public class MenuDetailService : BaseService<MenuDetail>, IMenuDetailService
    {
        public MenuDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<int> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<MenuDetail> GetByIdAsync(Guid id)
        {
            List<Expression<Func<MenuDetail, bool>>> filters = new()
            {
                (menuDetail) => menuDetail.Id == id
            };
            var menuDetail = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active, filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.MenuDetailMessageConstrant.MenuDetailNotFound(id));
            return menuDetail;
        }
    }
}
