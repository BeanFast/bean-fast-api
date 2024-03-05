using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Kitchen.Request
{
    public class KitchenFilterRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public Guid? AreaId { get; set; }
    }
}
