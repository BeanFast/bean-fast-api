using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Session.Request
{
    public class CreateSessionRequest
    {
        [RequiredGuid]
        public Guid MenuId { get; set; }
        [DateTimeGreaterThan("Now")]
        public DateTime OrderStartTime { get; set; }
        [DateTimeGreaterThan("OrderStartTime")]
        public DateTime OrderEndTime { get; set; }
        [DateTimeGreaterThan("OrderEndTime", additionalHours: TimeConstrant.GapBetweenOrderEndTimeAndDeliveryStartTimeInMinutes)]
        public DateTime DeliveryStartTime { get; set; }
        [DateTimeGreaterThan("DeliveryStartTime")]
        public DateTime DeliveryEndTime { get; set; }
        [RequiredListLength]
        public ICollection<SessionDetailOfCreateSessionRequest> SessionDetails { get; set; }
        
        public class SessionDetailOfCreateSessionRequest
        {
            [RequiredGuid]
            public Guid LocationId { get; set; }
        }
    }
}
