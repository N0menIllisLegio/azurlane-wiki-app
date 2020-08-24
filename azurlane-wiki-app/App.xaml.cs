using ShowMeTheXAML;
using System.Threading;
using System.Windows;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            XamlDisplay.Init();
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            SplashScreen splashScreen = new SplashScreen();

            Thread thread = new Thread(() =>
            {
                splashScreen = new SplashScreen();
                splashScreen.Show();

                System.Windows.Threading.Dispatcher.Run();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            MainWindow mainWindow = new MainWindow();
            MainWindow = mainWindow;

            splashScreen.Dispatcher.InvokeShutdown();

            mainWindow.Show();
            mainWindow.Activate();
        }
    }
}
