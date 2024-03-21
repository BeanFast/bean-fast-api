using DataTransferObjects.Models.Menu.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Session.Response
{
    public class GetSessionForDeliveryResponse
    {
        public string? Code { get; set; }
        public DateTime DeliveryStartTime { get; set; }
        public DateTime DeliveryEndTime { get; set; }

        //public GetMenuResponse? Menu { get; set; }
    }
}
