using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace PiholeDashboard
{
    /// <summary>
    /// Uppercases the first letter of a string
    /// </summary>
    public class Capitalize : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;

            if (!(input is string))
                return $"Unknown";

            int len = input.Length;

            if (len == 0)
            {
                return $"Unknown";
            }

            var firstUpper = input.First().ToString().ToUpper() + input.Substring(1);

            return $"{firstUpper}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

