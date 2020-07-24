using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

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

            // Already created columns
            if (string.IsNullOrEmpty(columnName) || columnName == "Icon" || columnName == "Nationality"
                                                 || columnName == "Id"   || columnName == "Type"  || columnName == "Name"
                                                 || columnName == "Tech" || columnName == "Stars" || columnName == "Rarity"
                                                 || columnName == "NationalityIcon")
            {
                e.Cancel = true;
                return;
            }

            // Override header if attribute DisplayedName exists
            if (e.PropertyDescriptor is PropertyDescriptor descriptor)
            {
                displayedName = descriptor.DisplayName ?? descriptor.Name;
            }
            else
            {
                displayedName = columnName;
            }

            // Cell Template
            FrameworkElementFactory cellTemplate = new FrameworkElementFactory(typeof(TextBlock));
            cellTemplate.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
            cellTemplate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            cellTemplate.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            cellTemplate.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            cellTemplate.SetBinding(TextBlock.TextProperty, new Binding(columnName));

            // Header Template
            FrameworkElementFactory headerTemplate = new FrameworkElementFactory(typeof(TextBlock));
            headerTemplate.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            headerTemplate.SetValue(TextBlock.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
            headerTemplate.SetValue(TextBlock.TextWrappingProperty, TextWrapping.Wrap);
            headerTemplate.SetValue(TextBlock.TextProperty, displayedName);
            headerTemplate.SetValue(TextBlock.TextAlignmentProperty, TextAlignment.Center);
            

            // Create cells and headers styles
            Style cellStyle = new Style(typeof(DataGridCell));
            cellStyle.BasedOn = Application.Current.Resources["MyDataGridCell"] as Style;
            Style headerStyle = new Style(typeof(DataGridColumnHeader));
            headerStyle.BasedOn = Application.Current.Resources["MaterialDesignDataGridColumnHeader"] as Style;
            headerStyle.Setters.Add(new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));

            // Marks DPS cells and headers with color
            if (columnName.Contains("DPS"))
            {
                SolidColorBrush cellColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#fff0e8");
                SolidColorBrush headerColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#ffdddd");

                cellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, cellColor));
                cellStyle.Setters.Add(new Setter(DataGridCell.BorderThicknessProperty, new Thickness(0, 0, 0, 0)));
                cellStyle.Setters.Add(new Setter(DataGridCell.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));

                headerStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, headerColor));
            }

            var col = new DataGridTemplateColumn
            {
                CellTemplate = new DataTemplate { VisualTree = cellTemplate },
                CellStyle = cellStyle,
                MaxWidth = columnName.Contains("Note") ? 300 : Double.PositiveInfinity,
                HeaderTemplate = new DataTemplate { VisualTree = headerTemplate },
                HeaderStyle = headerStyle,
                CanUserSort = true,
                SortMemberPath = columnName
            };

            e.Column = col;
        }
    }
}
