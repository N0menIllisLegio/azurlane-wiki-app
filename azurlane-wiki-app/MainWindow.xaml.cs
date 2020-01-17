using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace azurlane_wiki_app
{
    public class FAConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = "";
            
            if (parameter.ToString() == "Icon")
            {
                switch ((Statuses)value)
                {
                    case Statuses.InProgress:
                        output = "Cog";
                        break;
                    case Statuses.ErrorInDeserialization:
                        output = "Stop";
                        break;
                    case Statuses.DownloadComplete:
                        output = "Download";
                        break;
                    case Statuses.Pending:
                        output = "WPForms";
                        break;
                }
            }
            else
            {
                switch ((Statuses)value)
                {
                    case Statuses.InProgress:
                        output = "True";
                        break;
                    case Statuses.ErrorInDeserialization:
                    case Statuses.DownloadComplete:
                    case Statuses.Pending:
                        output = "False";
                        break;
                }
            }
            

            return output;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random;

        private ShipDownloader shipDownloader = new ShipDownloader();
        private EquipmentDownloader equipmentDownloader = new EquipmentDownloader();
        private SkillDownloader skillDownloader = new SkillDownloader();
        private WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

        public MainWindow()
        {
            InitializeComponent(); 
            random = new Random();

            StatusGrid.Visibility = Visibility.Hidden;

            DropSpinner.DataContext = wtgShipGirlDownloader;

            SkillCurrentCount.DataContext = SkillSpinner.DataContext =
                SkillDownloadProgress.DataContext = SkillTotalCount.DataContext = skillDownloader;

            ShipCurrentCount.DataContext = ShipSpinner.DataContext =
                ShipDownloadProgress.DataContext = ShipTotalCount.DataContext = shipDownloader;

            EquipCurrentCount.DataContext = EuipSpinner.DataContext =
                EquipDownloadProgress.DataContext = EquipTotalCount.DataContext = equipmentDownloader;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            StatusGrid.Visibility = Visibility.Visible;

            await Task.WhenAll(
                shipDownloader.Download(),
                equipmentDownloader.Download()
            ).ContinueWith(delegate
                {
                    Task.WhenAll(
                        skillDownloader.Download(),
                        wtgShipGirlDownloader.Download()
                    );
                },
                TaskContinuationOptions.RunContinuationsAsynchronously);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                int randomNum = random.Next(1, 350);
                ShipGirl shipGirl = cargoContext.ShipGirls.Find(randomNum.ToString());

                if (shipGirl != null)
                {
                    InfoLabel.Content = "Girl Name:" + shipGirl.Name
                                                     + "\n" +
                                                     "Girl skill: " + (shipGirl.Skills.FirstOrDefault()?.Name ?? "Suka") 
                                                     + "\n" + 
                                                     "Girl Drop Location:\n" 
                                                     + (shipGirl.WhereToGetShipGirl.FirstOrDefault()?.FK_WhereToGetShipGirl.Name ?? "Suka");
                }
                else
                {
                    InfoLabel.Content = "Nothing to display.\nNumber: " + randomNum;
                }
            }
        }
    }
}
