using System;
using System.Collections.Generic;
using System.Linq;
using azurlane_wiki_app.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random;

        public MainWindow()
        {
            InitializeComponent(); 
            random = new Random();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShipDownloader shipDownloader = new ShipDownloader();
            EquipmentDownloader equipmentDownloader = new EquipmentDownloader();
            SkillDownloader skillDownloader = new SkillDownloader();
            WTGShipGirlDownloader wtgShipGirlDownloader = new WTGShipGirlDownloader();

            SkillCurrentCount.DataContext =
                SkillDownloadProgress.DataContext = SkillTotalCount.DataContext = skillDownloader;

            ShipCurrentCount.DataContext =
                ShipDownloadProgress.DataContext = ShipTotalCount.DataContext = shipDownloader;

            EquipCurrentCount.DataContext =
                EquipDownloadProgress.DataContext = EquipTotalCount.DataContext = equipmentDownloader;

            Task task = Task.WhenAll(
                shipDownloader.Download(),
                equipmentDownloader.Download()
                );

            task.ContinueWith(delegate
                {
                    Task.WhenAll(
                        skillDownloader.Download(),
                        wtgShipGirlDownloader.Download()
                    );
                },
                TaskContinuationOptions.RunContinuationsAsynchronously);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                int randomNum = random.Next(1, 350);
                ShipGirl shipGirl = cargoContext.ShipGirls.Find(randomNum.ToString());

                if (shipGirl != null)
                {
                    InfoLabel.Content = "Girl Name:" + shipGirl.Name
                                                     + "\n" +
                                                     "Girl skill: " + (shipGirl.Skills.FirstOrDefault()?.Name ?? "Suka") 
                                                     + "\n" + 
                                                     "Girl Drop Location:\n" 
                                                     + (shipGirl.WhereToGetShipGirl.FirstOrDefault()?.FK_WhereToGetShipGirl.Name ?? "Suka");
                }
                else
                {
                    InfoLabel.Content = "Nothing to display.\nNumber: " + randomNum;
                }
            }
        }
    }
}
