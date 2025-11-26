using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SoftPhone.ValueConvertor
{
    public class BoolNegateConvertor : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool bv) return !bv;
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool bv) return !bv;
            return false;
        }
    }
}
