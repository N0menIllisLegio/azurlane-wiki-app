using System.Windows;
using System.Windows.Controls;

namespace azurlane_wiki_app.PageDownload
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
        }

        public DownloadPage(DownloadPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void DownloadPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext == null)
            {
                DataContext = (Window.GetWindow(this) as MainWindow)?.DownloadPageVM;
            }
        }
    }
}
