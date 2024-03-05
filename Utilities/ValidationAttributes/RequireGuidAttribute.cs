using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.ValidationAttributes
{
    public class RequireGuidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || (Guid)value == Guid.Empty)
            {
                return new ValidationResult(MessageConstants.GuidMessageConstrant.GuidRequired);
            }
            return ValidationResult.Success;
        }
    }
}
