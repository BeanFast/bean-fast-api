using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Request
{
    public class OrderFilterRequest
    {
        public int? Status { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? SessionId { get; set; }
        public Guid? SessionDetailId { get; set; }
    }
}
