using System.Threading.Tasks;
using azurlane_wiki_app.Data.Downloaders;
using System.Windows;
using azurlane_wiki_app.Data;
using azurlane_wiki_app.PageEquipment;
using azurlane_wiki_app.PageShipGirl;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
            // DownloadMethod();

            using (CargoContext cargoContext = new CargoContext())
            {
                
                ShipGirlPageViewModel shipPageViewModel = 
                    new ShipGirlPageViewModel(cargoContext.ShipGirls.Find("246"));
                EquipmentPageViewModel equipmentPageViewModel =
                    new EquipmentPageViewModel(cargoContext.ShipGirlsEquipment.Find(363));

                // Main.Content = new ShipGirlPage(shipPageViewModel);
                Main.Content = new EquipmentPage(equipmentPageViewModel);
            }
        }

        private async void DownloadMethod()
        {
            IconDownloader iconDownloader = new IconDownloader();
            ShipDownloader shipDownloader = new ShipDownloader(6);
            EquipmentDownloader equipmentDownloader = new EquipmentDownloader(2);
            SkillDownloader skillDownloader = new SkillDownloader();
            WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

            await Task.Run(() => iconDownloader.Download());

            Task[] tasks = new Task[2];

            tasks[0] = Task.Run(() => shipDownloader.Download());
            tasks[1] = Task.Run(() => equipmentDownloader.Download());

            await Task.WhenAll(tasks);

            tasks[0] = Task.Run(() => skillDownloader.Download());
            tasks[1] = Task.Run(() => wtgShipGirlDownloader.Download());

            await Task.WhenAll(tasks);
        }
    }
}
