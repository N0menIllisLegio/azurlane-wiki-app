using System;
using System.Windows;
using System.Windows.Controls;

namespace azurlane_wiki_app.PageEquipment
{
    /// <summary>
    /// Interaction logic for EquipmentPage.xaml
    /// </summary>
    public partial class EquipmentPage : Page
    {
        public EquipmentPage()
        {
            InitializeComponent();
        }

        public EquipmentPage(EquipmentPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void StatsList_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 20;
            var col1 = 0.6;
            var col2 = 1 - col1;

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
        }
    }
}
