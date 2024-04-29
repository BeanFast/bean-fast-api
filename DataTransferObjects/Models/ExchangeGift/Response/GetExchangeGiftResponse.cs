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

        public class GiftOfGetExchangeGiftResponse
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
            
            public int Status { get; set; }
        }
    }
}
