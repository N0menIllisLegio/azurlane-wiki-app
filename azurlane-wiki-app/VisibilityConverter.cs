using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace azurlane_wiki_app
{
    class VisibilityConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int count = 0;

            foreach (object value in values)
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                {
                    count++;
                }
            }

            if (count == values.Length)
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
