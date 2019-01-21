using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace adrilight.ValidationRules
{
    class NumberRangeValidationRule : ValidationRule
    {
        public int? Minimum { get; set; }
        public int? Maximum { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = value as string;

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return ValidationResult.ValidResult;
            }

            if (!int.TryParse(stringValue.Trim(), out var intVal))
            {
                return new ValidationResult(false, "Please enter a number!");
            }

            if (Minimum.HasValue && intVal < Minimum.Value)
            {
                return new ValidationResult(false, $"Please enter a number greater or equal to {Minimum}!");
            }

            if (Maximum.HasValue && intVal > Maximum.Value)
            {
                return new ValidationResult(false, $"Please enter a number less than or equal to {Maximum}!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
