using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetTopBestSellerKitchenResponse
    {
        public string Name { get; set; }
        public int TotalOrder { get; set; }
        public int TotalItem { get; set; }
        public double Percentage { get; set; }  
    }
}
