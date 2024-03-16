using DataTransferObjects.Models.Location.Response;
using DataTransferObjects.Models.Session.Response;
using DataTransferObjects.Models.User.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.SessionDetail.Response
{
    public class GetSessionDetailResponse
    {
        public string? Code { get; set; }

        public GetLocationIncludeSchoolResponse? Location { get; set; }
        public GetSessionForDeliveryResponse? Session { get; set; }
        public GetDelivererResponse? Deliverer { get; set; }
    }
}
