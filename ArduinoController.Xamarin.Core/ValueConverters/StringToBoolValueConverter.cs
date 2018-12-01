using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ArduinoController.Xamarin.Core.ValueConverters
{
    public class StringToBoolValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBoolean(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool) value ? 1 : 0).ToString();
        }
    }
}
