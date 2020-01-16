using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

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
            ShipDownloader dd = new ShipDownloader();

            DownloadProgress.DataContext = dd;
            CurrentCount.DataContext = dd;
            TotalCount.DataContext = dd;

            dd.Download();

            //List<Task> tasks = new List<Task>();

            //tasks.Add(
            //    Task.Run(() =>
            //        {
            //            for (int i = 0; i < 900000; i++) ;
            //        }
            //    ));tasks.Add(
            //    Task.Run(() =>
            //        {
            //            for (int i = 0; i < 10000; i++) ;
            //        }
            //    ));tasks.Add(
            //    Task.Run(() =>
            //        {
            //            for (int i = 0; i < 101230000; i++) ;
            //        }
            //    ));tasks.Add(
            //    Task.Run(() =>
            //        {
            //            for (int i = 0; i < 1000; i++) ;
            //        }
            //    ));tasks.Add(
            //    Task.Run(() =>
            //        {
            //            for (int i = 0; i < 10440000; i++) ;
            //        }
            //    ));

            //Task.WaitAny(tasks.ToArray());
            //tasks.Add(Task.Run(() =>
            //    {
            //        for (int i = 0; i < 100000; i++) ;
            //    }
            //));
        }
    }
}
