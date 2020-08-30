using System;
using System.Globalization;
using Xamarin.Forms;

namespace PiholeDashboard
{
    public class ActiveClientNameConverter : IValueConverter
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

            if (!input.Contains("|"))
                return input;

            var splitVals = input.Split('|');

            return $"{splitVals[0]} ({splitVals[1]})";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

