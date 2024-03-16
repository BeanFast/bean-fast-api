using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Session.Request
{
    public class SessionFilterRequest
    {
        public bool Orderable { get; set; }
        public Guid MenuId { get; set; } = Guid.Empty;
        public DateTime? OrderStartTime { get; set; }
        public DateTime? OrderEndTime { get; set; }
        public DateTime? DeliveryStartTime { get; set; }
        public DateTime? DeliveryEndTime { get; set; }
        public int? Status { get; set; }

    }
}
