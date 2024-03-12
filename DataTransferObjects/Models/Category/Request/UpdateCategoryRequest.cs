using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;
using Utilities.Enums;
using Utilities.ValidationAttributes;

namespace DataTransferObjects.Models.Category.Request
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = MessageConstants.CategoryMessageConstrant.CategoryNameRequired)]
        [StringLength(200, MinimumLength = 10, ErrorMessage = MessageConstants.CategoryMessageConstrant.CategoryNameLength)]
        public string Name { get; set; }
        [RequiredFileExtensions(AllowedFileTypes.IMAGE)]
        public IFormFile Image { get; set; }
    }
}
