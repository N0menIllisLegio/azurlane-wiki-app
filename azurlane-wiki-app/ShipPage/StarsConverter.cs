using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace azurlane_wiki_app.ShipPage
{
    class StarsConverter : IMultiValueConverter
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

        //Multi
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
            {
                return null;
            }

            string rarity = values[0].ToString();
            string retrofitStatus = values[1].ToString();

            if (retrofitStatus == "True")
            {
                RarityConverter rarityConverter = new RarityConverter();
                rarity = rarityConverter.Convert(rarity, null, "-1", null)?.ToString();
            }

            if (string.IsNullOrEmpty(rarity) || !RarityStarsCount.ContainsKey(rarity))
            {
                return null;
            }

            int stars = RarityStarsCount[rarity];

            if (parameter == null)
            {
                // MAX
                return stars;
            }

            if (int.TryParse(parameter.ToString(), out int decrement))
            {
                return stars - decrement;
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
