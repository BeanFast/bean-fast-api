using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Category.Response
{
    public class GetTopSellerCategoryResponse
    {
        public string Category { get; set; }
        public double TotalSold { get; set; }
    }
}
