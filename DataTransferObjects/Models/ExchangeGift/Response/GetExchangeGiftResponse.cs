using DataTransferObjects.Models.OrderActivity.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.ExchangeGift.Response
{
    public class GetExchangeGiftResponse
    {
        public Guid Id { get; set;}
        public int Status { get; set; }
        public Guid SessionDetailId { get; set; }
        public Guid GiftId { get; set; }
        public string Code { get; set; }
        public int Points { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public GiftOfGetExchangeGiftResponse? Gift { get; set; }
        public SessionDetailOfExchangeGiftResponse? SessionDetail { get; set; }
        public ProfileOfExchangeGiftResponse? Profile { get; set; }
        public IList<GetOrderActivityResponse> OrderActivities { get; set; }
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
        public class SessionDetailOfExchangeGiftResponse
        {
            public string? Code { get; set; }
            public GetSessionOfSessionDetail? Session { get; set; }
            public LocationOfSessionDetail? Location { get; set; }

            public class GetSessionOfSessionDetail
            {
                public Guid Id { get; set; }
                public string Code { get; set; }
                public DateTime OrderStartTime { get; set; }
                public DateTime OrderEndTime { get; set; }
                public DateTime DeliveryStartTime { get; set; }
                public DateTime DeliveryEndTime { get; set; }
            }
            public class LocationOfSessionDetail
            {
                public string Name { get; set; }
                public SchoolOfLocation School { get; set; }
                public class SchoolOfLocation
                {
                    public string Name { get; set; }
                    public string Address { get; set; }
                    public AreaOfLocation Area { get; set; }
                    public class AreaOfLocation
                    {
                        public string City { get; set; }
                        public string District { get; set; }
                        public string Ward { get; set; }
                    }
                }
            }
        }
        public class ProfileOfExchangeGiftResponse
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
