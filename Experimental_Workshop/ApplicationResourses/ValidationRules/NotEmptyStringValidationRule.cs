using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Experimental_Workshop.ApplicationResourses.ValidationRules
{
    class NotEmptyStringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && value.GetType() != typeof(string))
                return new ValidationResult(false, "Input value was of the wrong type, expected a string");

            var Name = value as string;

            if (string.IsNullOrWhiteSpace(Name))
                return new ValidationResult(false, "This field cannot be empty or whitespace.");


            return new ValidationResult(true, null);
        }
    }
}
