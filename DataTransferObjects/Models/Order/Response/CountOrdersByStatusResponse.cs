using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class CountOrdersByStatusResponse
    {
        public int Status { get; set; }
        public int Count { get; set; }
        public int TotalRevenue { get; set; }
        public double Percentage { get; set; }
    }
}
