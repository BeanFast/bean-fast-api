using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Order.Response
{
    public class GetDelivererIdAndOrderCountBySessionDetailIdResponse
    {
        public Guid DelivererId { get; set; }
        public int OrderCount { get; set; } = 0;
        public HashSet<Guid> CustomerIds { get; set; } = new HashSet<Guid>();

        
    }
}
