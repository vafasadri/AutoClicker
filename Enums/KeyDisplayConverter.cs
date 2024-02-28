using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace AutoClicker.Enums
{
    public class KeyDisplayConverter : IValueConverter
    {
        private readonly KeyConverter keyConverter = new KeyConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => keyConverter.ConvertToString(KeyInterop.KeyFromVirtualKey((int)value));
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}
