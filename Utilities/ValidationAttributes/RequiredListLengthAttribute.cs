using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValidationAttributes
{
    public class RequiredListLengthAttribute : ValidationAttribute
    {
        private readonly int _max;
        private readonly int _min;

        public RequiredListLengthAttribute(int min = 1, int max = int.MaxValue)
        {
            _min = min;
            _max = max;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var list = value as IList;
            if (list == null || (list.Count > _max && list.Count < _min))
            {
                return new ValidationResult("bbb");
            }
            return ValidationResult.Success;
        }
    }
}
