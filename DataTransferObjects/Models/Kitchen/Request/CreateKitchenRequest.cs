using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Kitchen.Request
{
    public class CreateKitchenRequest
    {
        [RequiredGuid]
        public Guid AreaId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
        public string Address { get; set; }
    }
}
