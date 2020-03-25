using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace azurlane_wiki_app
{
    class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value?.ToString();

            if (path == null)
            {
                // TODO: image palceholder and MB here check image corruption
                return null;
            }

            return new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                           + path.Remove(0, 1)));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
