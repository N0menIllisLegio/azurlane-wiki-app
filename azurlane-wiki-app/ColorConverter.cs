using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace azurlane_wiki_app
{
    class ColorConverter : IMultiValueConverter
    {
        private static BrushConverter brushConverter = new BrushConverter();

        private Dictionary<string, Brush> TypeBrushes = new Dictionary<string, Brush>
        {
            ["AA Gun"] = Brushes.LightCoral,
            ["Auxiliary"] = Brushes.Gainsboro,
            ["BB Gun"] = Brushes.Pink,
            ["CA Gun"] = Brushes.NavajoWhite,
            ["CL Gun"] = Brushes.NavajoWhite,
            ["DD Gun"] = Brushes.PowderBlue,
            ["Fighter"] = Brushes.Violet,
            ["Torpedo"] = Brushes.DeepSkyBlue
        };

        private Dictionary<string, Brush> RarityBrushes = new Dictionary<string, Brush>
        {
            ["Normal"] = Brushes.Gainsboro,
            ["Rare"] = Brushes.PowderBlue,
            ["Elite"] = Brushes.Plum,
            ["Super Rare"] = Brushes.PaleGoldenrod,
            ["Priority"] = Brushes.PaleGoldenrod
        };

        private Dictionary<string, Brush> NationalityBrushes = new Dictionary<string, Brush>
        {
            ["Eagle Union"] = Brushes.PowderBlue,
            ["Eastern Radiance"] = Brushes.Plum,
            ["Iris Libre"] = Brushes.Gold,
            ["Ironblood"] = Brushes.Pink,
            ["North Union"] = Brushes.WhiteSmoke,
            ["Sakura Empire"] = Brushes.LavenderBlush,
            ["Universal"] = Brushes.Gainsboro,

            ["Bilibili"] = (Brush) brushConverter.ConvertFrom("#75B6D8"),
            ["Hololive"] = (Brush) brushConverter.ConvertFrom("#8EE7F1"),
            ["KizunaAI"] = (Brush) brushConverter.ConvertFrom("#FBA5BB"),
            ["Neptunia"] = (Brush) brushConverter.ConvertFrom("#B39AE5"),
            ["Royal Navy"] = (Brush) brushConverter.ConvertFrom("#83AAF0"),
            ["Sardegna Empire"] = (Brush) brushConverter.ConvertFrom("#6EBE93"),
            ["Utawarerumono"] = (Brush) brushConverter.ConvertFrom("#E48F94"),
            ["Vichya Dominion"] = (Brush) brushConverter.ConvertFrom("#D77C7C")
        };

        private Dictionary<string, Brush> ClassificationBrushes = new Dictionary<string, Brush>
        {
            ["Battleship"] = Brushes.Pink,
            ["Battlecruiser"] = Brushes.Pink,
            ["Aviation Battleship"] = Brushes.Pink,
            ["Large Cruiser"] = Brushes.Pink,
            ["Monitor"] = Brushes.NavajoWhite,
            ["Heavy Cruiser"] = Brushes.NavajoWhite,
            ["Light Cruiser"] = Brushes.NavajoWhite,
            ["Aviation Cruiser"] = Brushes.NavajoWhite,
            ["Torpedo Cruiser"] = Brushes.NavajoWhite,
            ["Aircraft Carrier"] = Brushes.Plum,
            ["Light Aircraft Carrier"] = Brushes.Plum,
            ["Destroyer"] = Brushes.PowderBlue,
            ["Submarine"] = Brushes.PaleGreen,
            ["Submarine Carrier"] = Brushes.PaleGreen,
            ["Repair Ship"] = Brushes.Aquamarine,
            ["Munition Ship"] = Brushes.Aquamarine
        };

        private Dictionary<string, Brush> SkillBrushes = new Dictionary<string, Brush>
        {
            ["Offense"] = Brushes.Pink,
            ["Offence"] = Brushes.Pink,
            ["Support"] = Brushes.Gold,
            ["Defense"] = Brushes.DeepSkyBlue,
            ["Defence"] = Brushes.DeepSkyBlue
        };

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string target; 
            string brushPicker;

            if (values.Length < 1)
            {
                return Brushes.Transparent;
            }

            if (parameter != null)
            {
                target = parameter.ToString();
                brushPicker = values[0]?.ToString();
            }
            else
            {
                target = values[0]?.ToString();
                brushPicker = values[1]?.ToString();
            }

            if (target == null 
                || brushPicker == null
                || target == DependencyProperty.UnsetValue.ToString()
                || brushPicker ==  DependencyProperty.UnsetValue.ToString())
            {
                return Brushes.Transparent;
            }

            if (target.Equals("Type"))
            {
                return TypeBrushes[brushPicker];
            }

            if (target.Equals("Rarity"))
            {
                if (brushPicker.Equals("Ultra Rare") || brushPicker.Equals("Decisive")
                                                     || brushPicker.Equals("Legendary"))
                {
                    LinearGradientBrush brush = new LinearGradientBrush();

                    Color green = (Color) System.Windows.Media.ColorConverter.ConvertFromString("#AFA");
                    Color blue = (Color) System.Windows.Media.ColorConverter.ConvertFromString("#AAF");
                    Color red = (Color) System.Windows.Media.ColorConverter.ConvertFromString("#FAA");

                    brush.StartPoint = new Point(0, 0);
                    brush.EndPoint = new Point(1, 1);
                    brush.GradientStops.Add(new GradientStop(green, 0));
                    brush.GradientStops.Add(new GradientStop(blue, 0.5));
                    brush.GradientStops.Add(new GradientStop(red, 1));

                    return brush;
                }
                else
                {
                    return RarityBrushes[brushPicker];
                }
            }

            if (target.Equals("Nationality"))
            {
                return NationalityBrushes[brushPicker];
            }

            if (target.Equals("Classification"))
            {
                return ClassificationBrushes[brushPicker];
            }

            if (target.Equals("Skill"))
            {
                return SkillBrushes[brushPicker];
            }

            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
