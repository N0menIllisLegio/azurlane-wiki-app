using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace azurlane_wiki_app
{
    class ImagePathConverter : IValueConverter
    {
        public static BitmapImage ImagePlaceholder
        {
            get
            {
                if (bc_ImagePlaceholder == null)
                {
                    BitmapImage image = new BitmapImage(new Uri(@"pack://application:,,,/Resources/Placeholder.png"));
                    image.Freeze();
                    Application.Current.Dispatcher.Invoke(new Action(() => bc_ImagePlaceholder = image));
                }

                return bc_ImagePlaceholder;
            }
        }

        private static Dictionary<string, BitmapImage> icons = new Dictionary<string, BitmapImage>();
        private static BitmapImage bc_ImagePlaceholder = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value?.ToString();

            if (path == null)
            {
                return null;
            }

            BitmapImage outputImage;

            if (path.Contains("./Images/Icons/"))
            {
                outputImage = GetIcon(path);
            }
            else
            {
                if (!(new ImageChecker(path).IsComplete))
                {
                    outputImage = ImagePlaceholder;
                }
                else
                {
                    try
                    {
                        outputImage = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                              + path.Remove(0, 1)));
                    }
                    catch
                    {
                        outputImage = null;
                        Logger.Write($"Failed to display image. Path: {path}", this.GetType().ToString());
                    }
                }
            }

            return outputImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private BitmapImage GetIcon(string iconPath)
        {
            BitmapImage bitmapImage;

            if (!icons.ContainsKey(iconPath))
            {
                if (!(new ImageChecker(iconPath).IsComplete))
                {
                    return ImagePlaceholder;
                }

                bitmapImage = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                      + iconPath.Remove(0, 1)));
                icons.Add(iconPath, bitmapImage);
            }
            else
            {
                bitmapImage = icons[iconPath];
            }

            return bitmapImage;
        }
    }
}
