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

        public BitmapImage ImageShipyardIcon 
        { 
            get => imageShipyardIcon;
            set
            {
                imageShipyardIcon = value;
                OnPropertyChanged(nameof(ImageShipyardIcon));
            }
        }

        public BitmapImage bufImageShipyardIcon;

        public GraphicShipGirlItem(ShipGirl shipGirl, string name = null, string iconPath = null) : base(shipGirl, name, iconPath)
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
                Logger.Write($"Failed to load image. path: {shipGirl.ImageShipyardIcon}", this.GetType().ToString());
            }
            
            image.Freeze();
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => ImageShipyardIcon = image));

            if (Remodel)
            {
                BitmapImage imageKai;

                try
                {
                    imageKai = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                          + shipGirl.ImageShipyardIconKai.Remove(0, 1)));
                }
                catch
                {
                    imageKai = ImagePathConverter.ImagePlaceholder;
                    Logger.Write($"Failed to load image. path: {shipGirl.ImageShipyardIconKai}", this.GetType().ToString());
                }

                imageKai.Freeze();
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                        new Action(() => bufImageShipyardIcon = imageKai));
            }
        }

        public override void InvertRetrofit()
        {
            base.InvertRetrofit();

            if (Remodel)
            {
                ImageShipyardIcon = Swap(ImageShipyardIcon, ref bufImageShipyardIcon);
            }
        }
    }
}
