using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Location.Request
{
    public class BestSellerLocationFilterRequest
    {
        public Guid? SchoolId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Number { get; set; } = 5;
    }
}
