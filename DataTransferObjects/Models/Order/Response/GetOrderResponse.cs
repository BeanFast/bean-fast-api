using DataTransferObjects.Models.Location.Response;
using DataTransferObjects.Models.Order.Request;
using DataTransferObjects.Models.OrderActivity.Response;
using DataTransferObjects.Models.Profiles.Response;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.SessionDetail.Response;
using DataTransferObjects.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public double TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int RewardPoints { get; set; }
        public int Status { get; set; }
        public string? Feedback { get; set; }
        public ProfileOfOrderRessponse? Profile { get; set; }
        public SessionDetailOfOrderResponse? SessionDetail { get; set; }
        public IList<GetOrderActivityResponse> OrderActivities { get; set; }
        public IList<OrderDetailOfGetOrderResponse> OrderDetails { get; set; }
        public class OrderDetailOfGetOrderResponse
        {
            public Guid OrderId { get; set; }
            public FoodOfOrderDetail Food { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public string? Note { get; set; }
            public class FoodOfOrderDetail
            {
                public Guid Id { get; set; }
                public string Code { get; set; }
                public string Name { get; set; }
                public double Price { get; set; }
                public string Description { get; set; }
                public bool IsCombo { get; set; }
                public string ImagePath { get; set; }
                public CategoryOfFood Category { get; set; }
                public class CategoryOfFood
                {
                    public Guid Id { get; set; }
                    public string Name { get; set; }
                }

            }
        }
        public class SessionDetailOfOrderResponse
        {
            public string? Code { get; set; }
            public Guid Id { get; set; }
            public GetSessionOfSessionDetail? Session { get; set; }

            public class GetSessionOfSessionDetail
            {
                public Guid Id { get; set; }
                public string Code { get; set; }
                public DateTime OrderStartTime { get; set; }
                public DateTime OrderEndTime { get; set; }
                public DateTime DeliveryStartTime { get; set; }
                public DateTime DeliveryEndTime { get; set; }
            }
        }

        public class ProfileOfOrderRessponse
        {
            public string FullName { get; set; }
            public string? NickName { get; set; }
            public string AvatarPath { get; set; }
            public DateTime Dob { get; set; }
            public string? Class { get; set; }
            public bool Gender { get; set; }
        }
    }
}
