using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Utils;

namespace Utilities.ValidationAttributes
{
    public class DateTimeGreaterThanAttribute : ValidationAttribute
    {
        private readonly string? _comparisonProperty;
        private readonly int _additionalMinutes;
        private readonly DateTime? _comparisionTime;
        //public DateTimeGreaterThanAttribute(DateTime comparisionTime, int additionalHours = 0)
        //{
        //    _comparisionTime = comparisionTime;
        //    _additionalHours = additionalHours;
        //}
        public DateTimeGreaterThanAttribute(string comparisonProperty, int additionalMinutes = 0)
        {
            Console.WriteLine(_comparisonProperty);

            if (comparisonProperty != "Now")
            {
                _comparisonProperty = comparisonProperty;

            }
            else
            {
                _comparisonProperty = null;
                _comparisionTime = TimeUtil.GetCurrentVietNamTime();
            }
            _additionalMinutes = additionalMinutes;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Console.WriteLine(_comparisonProperty);
            //Console.WriteLine(currentValue);
            Console.WriteLine(_comparisionTime);
            var currentValue = (DateTime)value!;

            if (_comparisonProperty != null)
            {
                //Console.WriteLine(_comparisionTime!.Value.AddHours(_additionalHours));

                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException("Property with this name not found");

                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance)!;

                if (currentValue < comparisonValue.AddMinutes(_additionalMinutes))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
            else
            {
                
                if (currentValue <= _comparisionTime!.Value.AddHours(_additionalMinutes))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
        }

    }
}
