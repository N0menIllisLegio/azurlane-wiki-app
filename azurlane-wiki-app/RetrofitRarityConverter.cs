using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace azurlane_wiki_app
{
    class RetrofitRarityConverter : IValueConverter
    {
        private readonly List<string> Rarities = new List<string>
        {
            "Normal", "Rare", "Elite", "Super Rare", "Ultra Rare", "Priority", "Decisive"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            string rarity = value as string;
            bool isRetrofited = (bool) parameter;

            if (!isRetrofited)
            {
                return rarity;
            }

            int currentRarityIndex = Rarities.IndexOf(rarity);

            return Rarities[++currentRarityIndex];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
