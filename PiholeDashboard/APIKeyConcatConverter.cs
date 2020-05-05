using System;
using System.Globalization;
using Xamarin.Forms;

namespace PiholeDashboard
{
    /// <summary>
    /// Obfuscates the displayed API key.
    /// </summary>
    public class APIKeyConcatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string input = (string)value;
            int len = input.Length;

            if (len == 0)
            {
                return $"No Key Provided.";
            }

            return $"{input.Substring(0, Math.Min(len / 2, 10))}XXXXXXXXX";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

