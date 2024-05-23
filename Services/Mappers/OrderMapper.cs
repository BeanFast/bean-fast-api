﻿using BusinessObjects.Models;
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
            CreateMap<Order, GetOrderResponse>().ForMember(src => src.OrderActivities, opt => opt.MapFrom(dest => dest.OrderActivities!.OrderByDescending(oa => oa.Time)));
            CreateMap<OrderDetail, GetOrderResponse.OrderDetailOfGetOrderResponse>();
            CreateMap<Food, GetOrderResponse.OrderDetailOfGetOrderResponse.FoodOfOrderDetail>();
            CreateMap<Category, GetOrderResponse.OrderDetailOfGetOrderResponse.FoodOfOrderDetail.CategoryOfFood>();
            CreateMap<SessionDetail, GetOrderResponse.SessionDetailOfOrderResponse>();
            CreateMap<Session, GetOrderResponse.SessionDetailOfOrderResponse.GetSessionOfSessionDetail>();
            CreateMap<Location, GetOrderResponse.SessionDetailOfOrderResponse.LocationOfSessionDetail>();
            CreateMap<School, GetOrderResponse.SessionDetailOfOrderResponse.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Profile, GetOrderResponse.ProfileOfOrderRessponse>();

            CreateMap<Order, GetOrderByIdResponse>();
            CreateMap<OrderDetail, GetOrderByIdResponse.OrderDetailOfGetOrderResponse>();
            CreateMap<SessionDetail, GetOrderByIdResponse.SessionDetailOfOrderResponse>();
            CreateMap<Profile, GetOrderByIdResponse.ProfileOfOrderRessponse>();
            CreateMap<Session, GetOrderByIdResponse.SessionDetailOfOrderResponse.GetSessionOfSessionDetail>();
            CreateMap<Location, GetOrderByIdResponse.SessionDetailOfOrderResponse.LocationOfSessionDetail>();
            CreateMap<School, GetOrderByIdResponse.SessionDetailOfOrderResponse.LocationOfSessionDetail.SchoolOfLocation>();
            CreateMap<Area, GetOrderByIdResponse.SessionDetailOfOrderResponse.LocationOfSessionDetail.SchoolOfLocation.AreaOfLocation>();
            CreateMap<CreateOrderRequest, Order>();
            CreateMap<CreateOrderRequest.OrderDetailOfCreateOrderRequest, OrderDetail>();
            
        }
    }
}
