using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Gift.Request
{
    public class GiftFilterRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? Points { get; set; }
    }
}
