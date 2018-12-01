using System;
using System.Globalization;
using MvvmCross.Converters;

namespace ArduinoController.Xamarin.Core.ValueConverters
{
    public class StringToByteValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToByte(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }
    }
}
