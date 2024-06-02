using BusinessObjects.Models;
using DataTransferObjects.Models.Location.Response;
using DataTransferObjects.Models.Menu.Response;
using DataTransferObjects.Models.OrderActivity.Response;
using DataTransferObjects.Models.School.Response;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataTransferObjects.Models.ExchangeGift.Response.GetExchangeGiftResponse;

namespace DataTransferObjects.Models.SessionDetail.Response
{
    public class GetSessionDetailResponse
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public LocationOfSessionDetail? Location { get; set; }
        public SessionOfSessionDetail? Session { get; set; }
        public ICollection<OrderOfSessionDetail>? Orders { get; set; }
        public ICollection<GetDelivererResponse>? Deliverers { get; set; }
        public ICollection<ExchangeGiftOfSessionDetail>? ExchangeGifts { get; set; }
        public class LocationOfSessionDetail
        {
            public Guid Id { get; set; }
            public string? Code { get; set; }
            public string? Name { get; set; }
            public string? Description { get; set; }
            public string? ImagePath { get; set; }
            public SchoolOfLocation? School { get; set; }
            public class SchoolOfLocation
            {
                public string Code { get; set; }
                public string Name { get; set; }
                public string Address { get; set; }
                public string ImagePath { get; set; }
                public AreaOfSchool? Area { get; set; }

                public class AreaOfSchool
                {
                    public string Code { get; set; }
                    public string City { get; set; }
                    public string District { get; set; }
                    public string Ward { get; set; }
                }
            }
        }
        public class SessionOfSessionDetail
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public DateTime OrderStartTime { get; set; }
            public DateTime OrderEndTime { get; set; }
            public DateTime DeliveryStartTime { get; set; }
            public DateTime DeliveryEndTime { get; set; }
        }
        public class ExchangeGiftOfSessionDetail
        {
            public Guid Id { get; set; }
            public int Status { get; set; }
            public Guid GiftId { get; set; }
            public string Code { get; set; }
            public int Points { get; set; }
            public DateTime PaymentDate { get; set; }
            public DateTime? DeliveryDate { get; set; }
            public ProfileGetSessionDetailResponse Profile { get; set; }
            public GiftOfGetExchangeGiftResponse? Gift { get; set; }
            public class GiftOfGetExchangeGiftResponse
            {
                public Guid Id { get; set; }
                public string Code { get; set; }
                public string Name { get; set; }
                public int Points { get; set; }
                public int InStock { get; set; }
                public string ImagePath { get; set; }
                public string Description { get; set; }
                public int Status { get; set; }
            }
        }
        public class OrderOfSessionDetail
        {
            public Guid Id { get; set; }
            public string Code { get; set; }
            public double TotalPrice { get; set; }
            public DateTime PaymentDate { get; set; }
            public Guid DelivererId { get; set; }
            public int Status { get; set; }
            public ICollection<OrderDetailOfOrder>? OrderDetails { get; set; }
            public ProfileGetSessionDetailResponse Profile { get; set; }
            public class OrderDetailOfOrder
            {
                
                public int Quantity { get; set; }
                public double Price { get; set; }
                public string? Note { get; set; }
                public FoodOfOrderDetail Food { get; set; }
                public class FoodOfOrderDetail
                {
                    public Guid Id { get; set; }
                    public string Name { get; set; }
                    public string ImagePath { get; set; }
                }
            }
        }
        public class ProfileGetSessionDetailResponse
        {
            public string FullName { get; set; }
            public string? NickName { get; set; }
            public string AvatarPath { get; set; }
            public DateTime Dob { get; set; }
            public string? Class { get; set; }
            public bool Gender { get; set; }
            public UserOfProfile User { get; set; }
            public class UserOfProfile
            {
                //public int Id { get; set; }
                public string FullName { get; set; }
            }
        }
    }
}
