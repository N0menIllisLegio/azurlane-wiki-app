using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.Data.Tables;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using azurlane_wiki_app.Data;

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

        public MainWindow()
        {
            InitializeComponent(); 
            random = new Random();

            StatusGrid.Visibility = Visibility.Hidden;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ShipDownloader shipDownloader = new ShipDownloader(6);
            EquipmentDownloader equipmentDownloader = new EquipmentDownloader(2);
            SkillDownloader skillDownloader = new SkillDownloader();
            WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

            DropSpinner.DataContext = wtgShipGirlDownloader;

            SkillCurrentCount.DataContext = SkillSpinner.DataContext =
                SkillDownloadProgress.DataContext = SkillTotalCount.DataContext = skillDownloader;

            ShipCurrentCount.DataContext = ShipSpinner.DataContext =
                ShipDownloadProgress.DataContext = ShipTotalCount.DataContext = shipDownloader;

            EquipCurrentCount.DataContext = EuipSpinner.DataContext =
                EquipDownloadProgress.DataContext = EquipTotalCount.DataContext = equipmentDownloader;

            StatusGrid.Visibility = Visibility.Visible;
            
            Task[] tasks = new Task[2];

            tasks[0] = Task.Run(() => shipDownloader.Download());
            tasks[1] = Task.Run(() => equipmentDownloader.Download());

            await Task.WhenAll(tasks);

            tasks[0] = Task.Run(() => skillDownloader.Download());
            tasks[1] = Task.Run(() => wtgShipGirlDownloader.Download());

            await Task.WhenAll(tasks);
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                int randomNum = random.Next(1, 350);
                ShipGirl shipGirl = await cargoContext.ShipGirls.FindAsync("001");//randomNum.ToString());

                if (shipGirl != null)
                {
                    InfoLabel.Content = "Girl Name: " + shipGirl.Name
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

        private async void ActionEvent(object sender, RoutedEventArgs e)
        {
            ShipDownloader shipDownloader = new ShipDownloader();
            EquipmentDownloader equipmentDownloader = new EquipmentDownloader(2);
            SkillDownloader skillDownloader = new SkillDownloader();
            WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();
            IconDownloader iconDownloader = new IconDownloader();

            //await seeder.DownloadIcons();

            //await shipDownloader.Download();
            //await iconDownloader.Download();
            //await shipDownloader.Download("001");
            //await equipmentDownloader.Download("Z Flag");
            //await skillDownloader.Download("Martyr");
            //await wtgShipGirlDownloader.Download("363");

            using (CargoContext cargoContext = new CargoContext())
            {
                //    var ship = cargoContext.ShipGirls.Find("001");
                //    await skillDownloader.Download(ship, cargoContext);
                //InfoLabel.Content = cargoContext.Icons.Count();
            }
        }
    }
}
