using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetOrdersByLastDaysResponse
    {
        public DateTime DateTime { get; set; }
        public string Day { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Revenue { get; set; }
    }
}
