using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfAnimatedGif;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        public SplashScreen()
        {
            Random random = new Random();

            string[] gifs = new string[] 
            {
                "akagi-cooks",
                "akashi",
                "amagi",
                "enter",
                "excited",
                "FDG",
                "gacha",
                "gaming",
                "hehe",
                "here-comes-akagi",
                "luffy",
                "mad-akagi",
                "numba-wan",
                "open-up",
                "panic",
                "portland",
                "scared",
                "uni"
            };

            InitializeComponent();

            BitmapImage gif = 
                new BitmapImage(
                    new Uri($"pack://application:,,,/Resources/SplashScreenGifs/{gifs[random.Next(gifs.Length - 1)]}.gif"));

            ImageBehavior.SetAnimatedSource(img, gif);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
