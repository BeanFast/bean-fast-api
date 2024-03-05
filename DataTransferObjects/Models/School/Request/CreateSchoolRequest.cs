using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.School.Request
{
    public class CreateSchoolRequest
    {
        public Guid AreaId { get; set; }
        //public Guid? KitchenId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IFormFile Image { get; set; }
    }
}
