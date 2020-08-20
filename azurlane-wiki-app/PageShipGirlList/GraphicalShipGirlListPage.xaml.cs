using System.Windows;
using System.Windows.Controls;

namespace azurlane_wiki_app.PageShipGirlList
{
    /// <summary>
    /// Interaction logic for GraphicalShipGirlListPage.xaml
    /// </summary>
    public partial class GraphicalShipGirlListPage : Page
    {
        public GraphicalShipGirlListPage()
        {
            InitializeComponent();
        }

        private void ShipGirlListPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Window.GetWindow(this) as MainWindow)?.GraphicalShipGirlListPageVM;
        }
    }
}
