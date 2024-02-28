using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoClicker.Utils
{
    internal class DoubleValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string data)) return new ValidationResult(false, "data is not a string");

            if (double.TryParse(data, out double result))
            {
                return ValidationResult.ValidResult;
            }
            else return new ValidationResult(false, "data is not a valid number");
        }
    }
}
