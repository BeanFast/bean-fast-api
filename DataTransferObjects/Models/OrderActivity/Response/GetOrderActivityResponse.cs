using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.OrderActivity.Response
{
    public class GetOrderActivityResponse
    {
        public Guid? OrderId { get; set; }
        public Guid? ExchangeGiftId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public string? ImagePath { get; set; }
    }
}
