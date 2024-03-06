using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.School.Request
{
    public class UpdateSchoolRequest
    {
        [RequiredGuid]
        public Guid AreaId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public IFormFile? Image { get; set; } = null;
    }
}
