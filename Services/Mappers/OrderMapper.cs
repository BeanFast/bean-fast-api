using BusinessObjects.Models;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.Order.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mappers
{
    public class OrderMapper : AutoMapper.Profile
    {
        public OrderMapper()
        {
            CreateMap<Order, GetOrderResponse>();
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<CreateOrderRequest.OrderDetailList, OrderDetail>();
            CreateMap<CreateOrderRequest.OrderActivityList, OrderActivity>();
            CreateMap<CreateOrderRequest.TransactionList, Transaction>();
        }
    }
}
