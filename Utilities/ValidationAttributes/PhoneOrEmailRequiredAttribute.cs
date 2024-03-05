using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.ValidationAttributes
{
    public class PhoneOrEmailRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phoneProperty = validationContext.ObjectType.GetProperty("Phone");
            var emailProperty = validationContext.ObjectType.GetProperty("Email");

            var phoneValue = phoneProperty?.GetValue(validationContext.ObjectInstance);
            var emailValue = emailProperty?.GetValue(validationContext.ObjectInstance);

            if (string.IsNullOrEmpty(phoneValue?.ToString()) && string.IsNullOrEmpty(emailValue?.ToString()))
            {
                var errorFieldList = new List<string>
                {
                    "Phone",
                    "Email"
                };
                var errorMessage = new ValidationResult(MessageConstants.LoginMessageConstrant.PhoneOrEmailRequired, errorFieldList);
                return errorMessage;
            }
            return ValidationResult.Success;
        }
    }
}
