using AutoMapper;
using BusinessObjects;
using BusinessObjects.Models;
using Microsoft.Extensions.Options;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Settings;
using Utilities.Statuses;
using Utilities.Utils;

namespace Services.Implements
{
    public class OrderDetailService : BaseService<OrderDetail>, IOrderDetailService
    {
        public OrderDetailService(IUnitOfWork<BeanFastContext> unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper, appSettings)
        {
        }

        public async Task CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            if (orderDetail == null)
            {
                throw new ArgumentNullException(nameof(orderDetail));
            }

            orderDetail.Status = OrderDetailStatus.Active;
            orderDetail.Id = Guid.NewGuid();
            await _repository.InsertAsync(orderDetail);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateOrderDetailListAsync(List<OrderDetail> orderDetails)
        {
            foreach (var orderDetail in orderDetails)
            {
                await CreateOrderDetailAsync(orderDetail);
            }
        }
    }
}
