using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Session.Request
{
    public class GetAvailableDelivererInDeliveryTimeRequest
    {
        [DateTimeGreaterThan("Now")]
        public DateTime DeliveryStartTime { get; set; }
        [DateTimeGreaterThan("DeliveryStartTime")]

        public DateTime DeliveryEndTime { get; set; }
    }
}
