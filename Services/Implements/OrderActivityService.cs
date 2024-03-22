using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Response;
using DataTransferObjects.Models.OrderActivity.Response;
using Microsoft.EntityFrameworkCore;
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
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;

namespace Services.Implements
{
    public class OrderActivityService : BaseService<OrderActivity>, IOrderActivityService
    {
        public OrderActivityService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task<OrderActivity> GetByIdAsync(Guid id)
        {
            List<Expression<Func<OrderActivity, bool>>> filters = new()
            {
                (orderActivity) => orderActivity.Id == id
            };
            var orderActivity = await _repository.FirstOrDefaultAsync(status: BaseEntityStatus.Active,
                filters: filters)
                ?? throw new EntityNotFoundException(MessageConstants.OrderActivityMessageConstrant.OrderActivityNotFound(id));
            return orderActivity;
        }

        public async Task<GetOrderActivityResponse> GetOrderActivityResponseByIdAsync(Guid id)
        {
            return _mapper.Map<GetOrderActivityResponse>(await GetByIdAsync(id));
        }
        public async Task CreateOrderActivityAsync(OrderActivity orderActivity)
        {
            if (orderActivity == null)
            {
                throw new ArgumentNullException(nameof(orderActivity));
            }

            orderActivity.Status = OrderActivityStatus.Active;
            orderActivity.Id = Guid.NewGuid();
            await _repository.InsertAsync(orderActivity);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateOrderActivityListAsync(List<OrderActivity> orderActivities)
        {
            foreach (var orderActivity in orderActivities)
            {
                await CreateOrderActivityAsync(orderActivity);
            }
        }
    }
}
