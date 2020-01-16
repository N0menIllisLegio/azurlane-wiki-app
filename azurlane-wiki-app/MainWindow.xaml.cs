using azurlane_wiki_app.Data;
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //ShipDownloader dd = new ShipDownloader();
            EquipmentDownloader dd = new EquipmentDownloader();

            DownloadProgress.DataContext = dd;
            CurrentCount.DataContext = dd;
            TotalCount.DataContext = dd;

            dd.Download();
        }
    }
}
