using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace AutoClicker.Utils
{
    public class ValueConverterGroup : List<IValueConverter>, IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return this.Aggregate(value, (current, converter) => converter.Convert(current, targetType, parameter, culture));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (this as IEnumerable<IValueConverter>).Reverse().Aggregate(value, (current, converter) => converter.ConvertBack(current, null, null, null));

        }

        #endregion
    }
}
