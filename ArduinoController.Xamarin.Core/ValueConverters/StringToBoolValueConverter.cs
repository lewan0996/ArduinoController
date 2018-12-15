using System;
using System.Globalization;
using MvvmCross.Binding.Extensions;
using MvvmCross.Converters;

namespace ArduinoController.Xamarin.Core.ValueConverters
{
    public class StringToBoolValueConverter : IMvxValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "1" : "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ConvertToBoolean();
        }
    }
}
