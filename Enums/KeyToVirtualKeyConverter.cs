using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;

namespace AutoClicker.Enums
{
    public class KeyToVirtualKeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return KeyInterop.KeyFromVirtualKey((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return KeyInterop.VirtualKeyFromKey((Key)value);
        }
    }
}
