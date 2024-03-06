using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Area.Request
{
    public class AreaFilterRequest
    {
        public string City { get; set; } = "";

        public string District { get; set; } = "";

        public string Ward { get; set; } = "";


    }
}
