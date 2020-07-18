using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class GraphicShipGirlItem : BaseShipGirlItem
    {
        private BitmapImage imageShipyardIcon;

        //public string ImageShipyardIcon { get; set; }
        //public string ImageShipyardIconKai { get; set; }
        public BitmapImage ImageShipyardIcon 
        { 
            get => imageShipyardIcon;
            set
            {
                imageShipyardIcon = value;
                OnPropertyChanged(nameof(ImageShipyardIcon));
            }
        }

        //public BitmapImage ImageShipyardIconKai { get; set; } 

        public GraphicShipGirlItem(ShipGirl shipGirl) : base(shipGirl)
        {
            BitmapImage image;

            try
            {
                image = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                      + shipGirl.ImageShipyardIcon.Remove(0, 1)));
            }
            catch
            {
                image = ImagePathConverter.ImagePlaceholder;
            }

            image.Freeze();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => ImageShipyardIcon = image));

            //if (!string.IsNullOrEmpty(shipGirl.ImageShipyardIconKai))
            //{
            //    try
            //    {
            //        ImageShipyardIconKai = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
            //                                              + shipGirl.ImageShipyardIconKai.Remove(0, 1)));
            //    }
            //    catch
            //    {
            //        ImageShipyardIconKai = ImagePathConverter.ImagePlaceholder;
            //    }
            //}

            //ImageShipyardIcon = shipGirl.ImageShipyardIcon;
            //ImageShipyardIconKai = shipGirl.ImageShipyardIconKai;
        }
    }
}
