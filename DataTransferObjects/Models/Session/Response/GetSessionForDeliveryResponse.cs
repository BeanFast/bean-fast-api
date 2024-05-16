using DataTransferObjects.Models.Menu.Response;
using DataTransferObjects.Models.OrderActivity.Response;
using DataTransferObjects.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Session.Response
{
    public class GetSessionForDeliveryResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public DateTime OrderStartTime { get; set; }
        public DateTime OrderEndTime { get; set; }
        public DateTime DeliveryStartTime { get; set; }
        public DateTime DeliveryEndTime { get; set; }
        public GetMenuResponse? Menu { get; set; }
        public ICollection<SessionDetailOfSession> SessionDetails { get; set; }
        public class SessionDetailOfSession
        {
            public Guid Id { get; set; }
            public LocationOfSessionDetail Location { get; set; }
            public ICollection<OrderOfSession> Orders { get; set; }
            public ICollection<GetDelivererResponse> Deliverers { get; set; }
            public class LocationOfSessionDetail
            {
                public Guid Id { get; set; }
                public string Name { get; set; }
                public Guid SchoolId { get; set; }
            }
            public class OrderOfSession
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

    }
}
