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
        public GetProfileResponse? Profile { get; set; }
        public SessionDetailOfOrderResponse? SessionDetail { get; set; }
        public IList<GetOrderActivityResponse> OrderActivities { get; set; }
        public IList<OrderDetailOfGetOrderResponse> OrderDetails { get; set; }
        public class OrderDetailOfGetOrderResponse
        {
            public Guid OrderId { get; set; }
            public Guid FoodId { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public string? Note { get; set; }
        }

        public class SessionDetailOfOrderResponse
        {
            public string? Code { get; set; }

            public GetLocationIncludeSchoolResponse? Location { get; set; }
            public GetSessionForDeliveryResponse? Session { get; set; }
            public GetDelivererResponse? Deliverer { get; set; }
        }
    }
}
