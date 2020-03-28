using azurlane_wiki_app.Data.Downloaders;
using System.Windows;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.ShipPage;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private Random random;

        public MainWindow()
        {
            InitializeComponent();
            // random = new Random();

            // StatusGrid.Visibility = Visibility.Hidden;
            CargoContext cargoContext = new CargoContext();

            //Button_Click(this, new RoutedEventArgs());
            Main.Content = new ShipGirlPage(cargoContext.ShipGirls.Find("246"));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            IconDownloader iconDownloader = new IconDownloader();
            ShipDownloader shipDownloader = new ShipDownloader(6);
            EquipmentDownloader equipmentDownloader = new EquipmentDownloader(2);
            SkillDownloader skillDownloader = new SkillDownloader();
            WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

            //SkillCurrentCount.DataContext = SkillDownloadProgress.DataContext 
            //    = SkillTotalCount.DataContext = skillDownloader;

            //ShipCurrentCount.DataContext = ShipDownloadProgress.DataContext 
            //    = ShipTotalCount.DataContext = shipDownloader;

            //EquipCurrentCount.DataContext = EquipDownloadProgress.DataContext 
            //    = EquipTotalCount.DataContext = equipmentDownloader;

            //StatusGrid.Visibility = Visibility.Visible;

            //await Task.Run(() => iconDownloader.Download());

            //Task[] tasks = new Task[2];

            //tasks[0] = Task.Run(() => shipDownloader.Download());
            //tasks[1] = Task.Run(() => equipmentDownloader.Download());

            //await Task.WhenAll(tasks);

            //tasks[0] = Task.Run(() => skillDownloader.Download());
            //tasks[1] = Task.Run(() => wtgShipGirlDownloader.Download());

            //await Task.WhenAll(tasks);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void ActionEvent(object sender, RoutedEventArgs e)
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
                //  var ship = cargoContext.ShipGirls.Find("001");
                //  await skillDownloader.Download(ship, cargoContext);
                //  InfoLabel.Content = cargoContext.Icons.Count();
            }
        }
    }
}
