using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Models.School.Response
{
    public class GetSchoolResponse
    {
        public Guid AreaId { get; set; }
        public Guid? KitchenId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ImagePath { get; set; }
    }
}
