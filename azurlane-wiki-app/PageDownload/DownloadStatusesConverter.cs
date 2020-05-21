using System;
using System.Globalization;
using System.Windows.Data;
using azurlane_wiki_app.Data.Downloaders;
using MaterialDesignThemes.Wpf;

namespace azurlane_wiki_app.PageDownload
{
    public class DownloadStatusesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Statuses status)
            {
                switch (status)
                {
                    case Statuses.DownloadComplete:
                        return PackIconKind.Check;
                    case Statuses.DownloadError:
                        break;
                    case Statuses.EmptyResponse:
                        break;
                    case Statuses.ErrorInDeserialization:
                        break;
                    case Statuses.InProgress:
                        return PackIconKind.Download;
                    case Statuses.Pending:
                        return PackIconKind.Cached;
                }
            }

            return PackIconKind.Error;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
