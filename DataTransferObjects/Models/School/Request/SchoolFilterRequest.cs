using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.School.Request
{
    public class SchoolFilterRequest
    {
        [StringLength(100, MinimumLength = 10, ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolCodeLength)]
        public string? Code { get; set; }
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolNameLength)]
        public string? Name { get; set; }
        [StringLength(500, ErrorMessage = MessageConstants.SchoolMessageConstrant.SchoolAddressLength)]
        public string? Address { get; set; }
        //[RequiredGuid]
        public Guid? AreaId { get; set; }

        public Guid? KitchenId { get; set; }

        public bool? Orderable { get; set; }
    }
}
