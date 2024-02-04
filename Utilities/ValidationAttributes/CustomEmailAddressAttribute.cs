using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.ValidationAttributes
{
    public class CustomEmailAddressAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                
                if (value is null)
                {
                    return null;
                }
                var emailAddress = (String)value;
                var email = new MailAddress(emailAddress);
                return ValidationResult.Success;
            }
            catch (FormatException)
            {
                return new ValidationResult(MessageConstants.Login.InvalidEmail);
            }
        }
    }
}
