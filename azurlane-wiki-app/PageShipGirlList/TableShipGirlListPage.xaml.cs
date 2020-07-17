using System.Windows;
using System.Windows.Controls;

namespace azurlane_wiki_app.PageShipGirlList
{
    /// <summary>
    /// Interaction logic for TableShipGirlListPage.xaml
    /// </summary>
    public partial class TableShipGirlListPage : Page
    {
        public TableShipGirlListPage()
        {
            InitializeComponent();
        }

        private void ShipGirlListPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Window.GetWindow(this) as MainWindow)?.TableShipGirlListPageVM;
        }
    }
}
