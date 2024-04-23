using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Models
{
    public class SessionDetailDeliverer : BaseAuditableEntity
    {
        public Guid DelivererId { get; set; }
        public virtual User? Deliverer{ get; set; }
        public Guid SessionDetailId { get; set; }
        public virtual SessionDetail? SessionDetail { get; set; }

    }
}
