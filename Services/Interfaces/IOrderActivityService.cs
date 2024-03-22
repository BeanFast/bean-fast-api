<<<<<<< HEAD
﻿using BusinessObjects.Models;
using DataTransferObjects.Models.OrderActivity.Response;
using System;
=======
﻿using System;
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
<<<<<<< HEAD
    public interface IOrderActivityService
    {
        Task<OrderActivity> GetByIdAsync(Guid id);

        Task<GetOrderActivityResponse> GetOrderActivityResponseByIdAsync(Guid id);
        Task CreateOrderActivityAsync(OrderActivity orderActivity);
        Task CreateOrderActivityListAsync(List<OrderActivity> orderActivities);
=======
    public interface IOrderActivityService : IBaseService
    {
>>>>>>> fc91342a0402d3445967991dcfd8a792b0fae0db
    }
}
