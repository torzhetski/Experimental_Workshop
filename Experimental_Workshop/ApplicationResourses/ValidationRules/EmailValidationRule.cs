using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Experimental_Workshop.ApplicationResourses.ValidationRules
{
    class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null && value.GetType() != typeof(string))
                return new ValidationResult(false, "Input value was of the wrong type, expected a string");

            var Email = value as string;

            if (Email.Length < 4)
                return new ValidationResult(false, "Email must be longer than 4.");
            if (!Email.Contains('.') || !Email.Contains('@') )
            {
                return new ValidationResult(false, "Email must contain '@' and '.'");
            }
            if(Email.IndexOf('.') < Email.IndexOf('@'))
            {
                return new ValidationResult(false,"'.' cannot be in front of '@'" );
            }

            return new ValidationResult(true, null);
        }
    }
}
