using System;
using System.Globalization;
using System.Windows.Data;

namespace azurlane_wiki_app.PageDownload
{
    public class PercentConverter : IMultiValueConverter
    {
        /// <summary>
        /// Gets percent
        /// </summary>
        /// <param name="values">First - Total amount, Second - Current amount</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && decimal.TryParse(values[0].ToString(), out var totalImage) 
                               && decimal.TryParse(values[1].ToString(), out var currentImage))
            {
                decimal.TryParse(values[2].ToString(), out var totalData);
                decimal.TryParse(values[3].ToString(), out var currentData);

                if (totalImage + totalData != 0)
                {
                    return Math.Ceiling((currentImage + currentData) / (totalImage + totalData) * 100).ToString();
                }
                else
                {
                    return "0";
                }
            }

            return "100";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
