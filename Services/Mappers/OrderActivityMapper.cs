using BusinessObjects.Models;
using DataTransferObjects.Models.OrderActivity.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class OrderActivityMapper : AutoMapper.Profile
    {
        public OrderActivityMapper()
        {
            CreateMap<CreateOrderActivityRequest, OrderActivity>();
            CreateMap<OrderActivity, GetOrderActivityResponse>();
        }
    }
}
