using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.ValidationAttributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? password = value as string;

            if (value == null || password == null || password.IsNullOrEmpty())
            {
                return new ValidationResult(MessageConstants.AuthorizationMessageConstrant.PasswordRequired);
            }

            
            if (!Regex.IsMatch(password, RegexConstants.PasswordRegex))
            {
                return new ValidationResult(MessageConstants.AuthorizationMessageConstrant.InvalidPassword);
            }

            return ValidationResult.Success;
        }
    }
}
