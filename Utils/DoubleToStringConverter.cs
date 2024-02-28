using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoClicker.Utils
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value != null)
            {
                return value.ToString();
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is string data))
            {
                return value;
            }        
            if (double.TryParse(data, out double result))
            {
                var str = result.ToString();
                if (str == data) return result;                               
            }
            return Binding.DoNothing;
        }
    }
}
