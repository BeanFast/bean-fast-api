using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.Kitchen.Request
{
    public class CreateKitchenRequest
    {
        public Guid AreaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
        public string Address { get; set; }
    }
}
