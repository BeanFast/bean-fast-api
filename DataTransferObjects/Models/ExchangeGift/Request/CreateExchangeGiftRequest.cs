using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.ExchangeGift.Request
{
    public class CreateExchangeGiftRequest
    {
        public Guid GiftId { get; set; }
        public Guid ProfileId { get; set; }
        public Guid SessionDetailId { get; set; }

    }
}
