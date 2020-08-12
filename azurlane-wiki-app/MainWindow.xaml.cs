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
        public EquipmentListPageViewModel EquipmentListPageVM { get; set; }
        public DownloadPageViewModel DownloadPageVM { get; set; }

        public TableShipGirlListPageViewModel TableShipGirlListPageVM { get; set; }
        public GraphicalShipGirlListPageViewModel GraphicalShipGirlListPageVM { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Navigation.Service = Main.NavigationService;

            TableShipGirlListPageVM = new TableShipGirlListPageViewModel();
            GraphicalShipGirlListPageVM = new GraphicalShipGirlListPageViewModel();

            EquipmentListPageVM = new EquipmentListPageViewModel();
            DownloadPageVM = new DownloadPageViewModel(this);

            Navigation.Navigate(new TableShipGirlListPage(), TableShipGirlListPageVM);
        }
    }
}
