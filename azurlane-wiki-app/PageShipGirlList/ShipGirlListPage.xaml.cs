using System.Windows;
using System.Windows.Controls;

namespace azurlane_wiki_app.PageShipGirlList
{
    /// <summary>
    /// Interaction logic for ShipGirlListPage.xaml
    /// </summary>
    public partial class ShipGirlListPage : Page
    {
        private bool IsGraphicalView;
        protected ShipGirlListPage()
        {
            InitializeComponent();
        }

        public ShipGirlListPage(bool isGraphicalView)
        {
            IsGraphicalView = isGraphicalView;
            InitializeComponent();
        }

        public ShipGirlListPage(ShipGirlListPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ShipGirlListPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IsGraphicalView)
            {
                (Window.GetWindow(this) as MainWindow)?.ShipGirlListPageVM.SetGraphicalView();
            }
            else
            {
                (Window.GetWindow(this) as MainWindow)?.ShipGirlListPageVM.SetTableView();
            }

            if (DataContext == null)
            {
                DataContext = (Window.GetWindow(this) as MainWindow)?.ShipGirlListPageVM;
            }
        }
    }
}
