using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetOrdersByLastMonthsResponse
    {
        public string Month { get; set; } = string.Empty;
        public int Count { get; set; }
        public int Revenue { get; set; }
    }
}
