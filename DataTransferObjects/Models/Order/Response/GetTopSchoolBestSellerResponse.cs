using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetTopSchoolBestSellerResponse
    {
        public string SchoolName { get; set; } = string.Empty;
        public int Count { get; set; }
        public double Percentage { get; set; }
        public int Revenue { get; set; }
    }
}
