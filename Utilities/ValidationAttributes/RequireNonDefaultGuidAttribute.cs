using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValidationAttributes
{
    public class RequireNonDefaultGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || (Guid)value == Guid.Empty)
            {
                return new ValidationResult();
            }
        }
    }
}
