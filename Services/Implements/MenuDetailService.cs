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
        private readonly IMenuDetailRepository _repository;
        public MenuDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings, IMenuDetailRepository repository) : base(unitOfWork, mapper, appSettings)
        {
            _repository = repository;
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

        public async Task HardDeleteAsync(List<MenuDetail> menuDetails)
        {
            foreach (var item in menuDetails)
            {
                await _repository.HardDeleteAsync(item);
            }
            await _unitOfWork.CommitAsync();
        }

        public async Task InsertRangeAsync(List<MenuDetail> menuDetails)
        {
            menuDetails.ForEach(async md =>
            {
                await _repository.InsertAsync(md);
            });
            await _unitOfWork.CommitAsync();
        }
    }
}
