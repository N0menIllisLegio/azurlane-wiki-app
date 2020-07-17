using System;
using System.Windows.Media.Imaging;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class GraphicShipGirlItem : BaseShipGirlItem
    {
        //public string ImageShipyardIcon { get; set; }

        //public string ImageShipyardIconKai { get; set; }
        public BitmapImage ImageShipyardIcon { get; set; }
        public BitmapImage ImageShipyardIconKai { get; set; }

        public GraphicShipGirlItem(ShipGirl shipGirl) : base(shipGirl)
        {
            try
            {
                ImageShipyardIcon = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                      + shipGirl.ImageShipyardIcon.Remove(0, 1)));
            }
            catch
            {
                ImageShipyardIcon = ImagePathConverter.ImagePlaceholder;
            }
            
            if (!string.IsNullOrEmpty(shipGirl.ImageShipyardIconKai))
            {
                try
                {
                    ImageShipyardIconKai = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                          + shipGirl.ImageShipyardIconKai.Remove(0, 1)));
                }
                catch
                {
                    ImageShipyardIconKai = ImagePathConverter.ImagePlaceholder;
                }
            }
            
            //ImageShipyardIcon = shipGirl.ImageShipyardIcon;
            //ImageShipyardIconKai = shipGirl.ImageShipyardIconKai;
        }
    }
}
