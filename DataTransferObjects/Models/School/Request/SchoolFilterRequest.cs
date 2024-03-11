using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.School.Request
{
    public class SchoolFilterRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        //[RequiredGuid]
        public Guid AreaId { get; set; }
        
    }
}
