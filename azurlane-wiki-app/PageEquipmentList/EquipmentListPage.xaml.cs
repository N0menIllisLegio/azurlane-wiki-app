using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace azurlane_wiki_app.PageEquipmentList
{
    /// <summary>
    /// Interaction logic for EquipmentListPage.xaml
    /// </summary>
    public partial class EquipmentListPage : Page
    {
        public EquipmentListPage()
        {
            InitializeComponent();
        } 
        
        public EquipmentListPage(EquipmentListPageViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void EquipmentListPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = (Window.GetWindow(this) as MainWindow)?.EquipmentListPageVM;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string displayedName, columnName = e.Column.Header.ToString();

            if (string.IsNullOrEmpty(columnName) || columnName == "Icon" || columnName == "Nationality"
                                                 || columnName == "Id"   || columnName == "Type"  || columnName == "Name"
                                                 || columnName == "Tech" || columnName == "Stars" || columnName == "Rarity"
                                                 || columnName == "NationalityIcon")
            {
                e.Cancel = true;
                return;
            }

            FrameworkElementFactory frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
            frameworkElementFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            frameworkElementFactory.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            frameworkElementFactory.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            frameworkElementFactory.SetBinding(TextBlock.TextProperty, new Binding(columnName));

            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                displayedName = descriptor.DisplayName ?? descriptor.Name;
            }
            else
            {
                displayedName = columnName;
            }

            var col = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate {VisualTree = frameworkElementFactory},
                MaxWidth = columnName.Contains("Note") ? 300 : Double.PositiveInfinity,
                Header = displayedName
            };

            e.Column = col;
        }
    }
}
