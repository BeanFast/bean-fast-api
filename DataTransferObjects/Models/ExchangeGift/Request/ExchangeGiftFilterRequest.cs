using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.ExchangeGift.Request
{
    public class ExchangeGiftFilterRequest
    {
        public string? Code { get; set; }
        public int?  Status { get; set; }
    }
}
