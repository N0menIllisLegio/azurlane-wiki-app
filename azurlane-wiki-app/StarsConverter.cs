using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace azurlane_wiki_app
{
    class StarsConverter : IValueConverter
    {
        private Dictionary<string, int> RarityStarsCount = new Dictionary<string, int>
        {
            ["Normal"] = 4,
            ["Rare"] = 5,
            ["Elite"] = 5,
            ["Super Rare"] = 6,
            ["Priority"] = 6,
            ["Decisive"] = 6,
            ["Ultra Rare"] = 6
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rarity = value?.ToString();

            if (rarity == null)
            {
                return 4;
            }

            int maxStars = RarityStarsCount[rarity];

            if (parameter == null)
            {
                return maxStars;
            }

            int decrement = int.Parse(parameter.ToString());

            return maxStars - decrement;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
