using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Category.Request
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
