using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Food.Request
{
    public class GetBestSellerFoodsRequest
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int Number { get; set; } = 5;
    }
}
