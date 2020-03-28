using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace azurlane_wiki_app
{
    class ImagePathConverter : IValueConverter
    {
        private const string ImagePlaceholder = "/Resources/Placeholder.png";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value?.ToString();
            

            if (path == null)
            {
                return null;
            }

            ImageChecker imageChecker = new ImageChecker(path);

            if (!imageChecker.IsComplete)
            {
                return new BitmapImage(new Uri(@"pack://application:,,," + ImagePlaceholder));
            }

            try
            {
                return new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                            + path.Remove(0, 1)));
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
