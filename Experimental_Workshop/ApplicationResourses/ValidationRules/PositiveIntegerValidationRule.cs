using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Experimental_Workshop.ApplicationResourses.ValidationRules
{
    class PositiveIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int posInt = 0;
            if (value != null && !int.TryParse((string)value,out posInt))
                return new ValidationResult(false, "Input value was of the wrong type, expected int");

            

            if (posInt <= 0)
                return new ValidationResult(false, "This value cannot be 0 and less");
            return new ValidationResult(true, null);
        }
    }
}
