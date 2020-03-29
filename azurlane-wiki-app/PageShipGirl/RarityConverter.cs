using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace azurlane_wiki_app.PageShipGirl
{
    class RarityConverter : IValueConverter
    {
        //  "Unreleased", "Priority", "Decisive"

        private readonly List<string> Rarities = new List<string>
        {
            "Normal", "Rare", "Elite", "Super Rare", "Ultra Rare"
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            string rarity = value.ToString();
            int shift = System.Convert.ToInt32(parameter);
            int current = Rarities.FindIndex(r => r.StartsWith(rarity));
            current += shift;

            if (current >= 0 && current <= 4)
            {
                return Rarities[current];
            }
            else
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
