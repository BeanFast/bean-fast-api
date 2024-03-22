using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrderDetailService : IBaseService
    {
        Task CreateOrderDetailAsync(OrderDetail orderDetail);
        Task CreateOrderDetailListAsync(List<OrderDetail> orderDetails);
    }
}
