using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Constants;

namespace Utilities.ValidationAttributes
{
    public class PaginationRequestValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var pageProperty = validationContext.ObjectType.GetProperty("Page");
            var sizeProperty = validationContext.ObjectType.GetProperty("Size");
            try
            {
                int pageValue = (int)pageProperty?.GetValue(validationContext.ObjectInstance)!;
                int sizeValue = (int)sizeProperty?.GetValue(validationContext.ObjectInstance)!;

                if (pageValue == 0 && sizeValue != 0)
                {
                    var errorFieldList = new List<string>
                    {
                        "Page"
                    };
                    var errorMessage = new ValidationResult(MessageContants.Pagination.PageRequired, errorFieldList);
                    return errorMessage;
                }
                else if (pageValue != 0 && sizeValue == 0)
                {
                    var errorFieldList = new List<string>
                    {
                        "Size"
                    };
                    var errorMessage = new ValidationResult(MessageContants.Pagination.SizeRequired, errorFieldList);
                    return errorMessage;
                }

                return ValidationResult.Success;
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ValidationResult(ex.Message);
            }
        }
    }
}
