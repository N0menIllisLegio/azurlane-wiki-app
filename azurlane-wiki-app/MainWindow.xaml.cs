using azurlane_wiki_app.PageDownload;
using azurlane_wiki_app.PageEquipmentList;
using azurlane_wiki_app.PageShipGirlList;
using System.Windows;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ShipGirlListPageViewModel ShipGirlListPageVM { get; set; }
        public EquipmentListPageViewModel EquipmentListPageVM { get; set; }
        public DownloadPageViewModel DownloadPageVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Navigation.Service = Main.NavigationService;

            ShipGirlListPageVM = new ShipGirlListPageViewModel();
            EquipmentListPageVM = new EquipmentListPageViewModel();
            DownloadPageVM = new DownloadPageViewModel(this);

            Navigation.Navigate(new ShipGirlListPage(false), ShipGirlListPageVM);
        }
    }
}
