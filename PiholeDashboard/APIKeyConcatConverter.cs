using System;
using System.Globalization;
using Xamarin.Forms;

namespace PiholeDashboard
{
    public class APIKeyConcatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;
            int len = input.Length;
            return $"{input.Substring(0, 10)}XXXXXXXXX";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "nope";
        }
    }
}

