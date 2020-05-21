using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace azurlane_wiki_app.PageShipGirl
{
    /// <summary>
    /// Interaction logic for ShipGirlPage.xaml
    /// </summary>
    public partial class ShipGirlPage : Page
    {
        public ShipGirlPage()
        {
            InitializeComponent();
        }

        public ShipGirlPage(ShipGirlPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void SkillsListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 20;
            var col1 = 0.07;
            var col2 = 0.20;
            var col3 = 1 - (col2 + col1);

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
            gView.Columns[2].Width = Math.Abs(workingWidth * col3);
        }

        private void EquipmentList_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 20;
            var col1 = 0.07;
            var col2 = 0.2;
            var col3 = 0.2;

            var col4 = 1 - (col1 + col2 + col3);

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
            gView.Columns[2].Width = Math.Abs(workingWidth * col3);
            gView.Columns[3].Width = Math.Abs(workingWidth * col4);
        }

        private void StatTable_OnEnter(object sender, MouseEventArgs e)
        {
            var element = (Border)e.Source;
            StatTable.BrushCell(Grid.GetRow(element), Grid.GetColumn(element), Brushes.LightGray);
        }

        private void StatTable_OnLeave(object sender, MouseEventArgs e)
        {
            var element = (Border)e.Source;
            StatTable.BrushCell(Grid.GetRow(element), Grid.GetColumn(element), Brushes.Transparent);
        }
    }
}
