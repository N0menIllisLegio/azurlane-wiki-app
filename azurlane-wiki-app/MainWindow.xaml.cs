using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.PageEquipment;
using azurlane_wiki_app.PageShipGirl;
using System.Threading.Tasks;
using System.Windows;

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
            //DownloadData();
            DisplayContent();
        }

        private void DisplayContent()
        {
            using (CargoContext cargoContext = new CargoContext())
            {

                ShipGirlPageViewModel shipPageViewModel =
                    new ShipGirlPageViewModel(cargoContext.ShipGirls.Find("155"));
                EquipmentPageViewModel equipmentPageViewModel =
                    new EquipmentPageViewModel(cargoContext.ShipGirlsEquipment.Find(364));

               // Main.Content = new ShipGirlPage(shipPageViewModel);
                Main.Content = new EquipmentPage(equipmentPageViewModel);
            }
        }

        private async void DownloadData()
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
