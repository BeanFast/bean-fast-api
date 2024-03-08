using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Enums;

namespace Utilities.ValidationAttributes
{
    public class RequiredFileExtensionsAttribute : ValidationAttribute
    {
        
        public RequiredFileExtensionsAttribute(params AllowedFileTypes[] AllowedFileTypes)
        {
            
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}
