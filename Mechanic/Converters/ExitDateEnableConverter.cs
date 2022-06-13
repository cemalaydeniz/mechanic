using System;
using System.Globalization;
using System.Windows.Data;


namespace Mechanic.Converters
{
    public class ExitDateEnableConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2 || values[0] is not bool || values[1] is not bool)
                return false;

            return (bool)values[0] && (bool)values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
