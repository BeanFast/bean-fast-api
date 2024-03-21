﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.ValidationAttributes
{
    public class DateTimeGreaterThanAttribute : ValidationAttribute
    {
        private readonly string? _comparisonProperty;
        private readonly int _additionalHours;
        private readonly DateTime? _comparisionTime;
        public DateTimeGreaterThanAttribute(DateTime comparisionTime, int additionalHours = 0)
        {
            _comparisionTime = comparisionTime;
            _additionalHours = additionalHours;
        }
        public DateTimeGreaterThanAttribute(string comparisonProperty, int additionalHours = 0)
        {
            if (comparisonProperty != "Now")
            {
                _comparisonProperty = comparisonProperty;

            }
            else
            {
                _comparisonProperty = null;
                _comparisionTime = DateTime.Now;
            }
            _additionalHours = additionalHours;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Console.WriteLine(_comparisonProperty);
            Console.WriteLine(_comparisionTime.ToString());
            var currentValue = (DateTime)value!;

            if (_comparisonProperty != null)
            {

                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

                if (property == null)
                    throw new ArgumentException("Property with this name not found");

                var comparisonValue = (DateTime)property.GetValue(validationContext.ObjectInstance)!;

                if (currentValue <= comparisonValue.AddHours(_additionalHours))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
            else
            {
                if (currentValue <= _comparisionTime!.Value.AddHours(_additionalHours))
                    return new ValidationResult(ErrorMessage);

                return ValidationResult.Success;
            }
        }

    }
}
