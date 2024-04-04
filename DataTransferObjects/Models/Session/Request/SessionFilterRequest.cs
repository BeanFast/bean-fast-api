using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Session.Request
{
    public class SessionFilterRequest
    {
        public Guid? MenuId { get; set; } = Guid.Empty;
        public DateTime? OrderStartTime { get; set; }
        public DateTime? OrderTime { get; set; }
        public DateTime? OrderEndTime { get; set; }
        public DateTime? DeliveryStartTime { get; set; }
        public DateTime? DeliveryEndTime { get; set; }
        public bool Orderable { get; set; }
        public bool Expired { get; set; }
        public bool Incomming { get; set; }
        public int? Status { get; set; }
        public Guid? SchoolId { get; set; }

    }
}
