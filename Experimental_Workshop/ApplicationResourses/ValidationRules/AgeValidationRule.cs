using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Experimental_Workshop.ApplicationResourses.ValidationRules
{
    class AgeValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime date = new DateTime();
            if (value != null && !DateTime.TryParse(value.ToString(),out date))
                return new ValidationResult(false, "Input value was of the wrong type, expected int");
            if (DateTime.Now.Year - date.Year >=100)
                return new ValidationResult(false, "Date Of Birth cant be more than 100 years ago");
            if (DateTime.Now.Year - date.Year <= 18)
                return new ValidationResult(false, "You cant add worker younger than 18");
            return new ValidationResult(true, null);
        }
    }
}
