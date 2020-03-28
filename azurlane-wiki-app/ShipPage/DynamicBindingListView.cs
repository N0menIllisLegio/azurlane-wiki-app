using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace azurlane_wiki_app.ShipPage
{
    public class DynamicBindingListView
    {

        public static bool GetGenerateColumnsGridView(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return (bool)element.GetValue(GenerateColumnsGridViewProperty);
        }

        public static void SetGenerateColumnsGridView(DependencyObject element, bool value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(GenerateColumnsGridViewProperty, value);
        }


        public static readonly DependencyProperty GenerateColumnsGridViewProperty
            = DependencyProperty.RegisterAttached("GenerateColumnsGridView",
                typeof(bool?), typeof(DynamicBindingListView),
                new FrameworkPropertyMetadata(null, thePropChanged));

        public static void thePropChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ListView lv = (ListView)obj;
            DependencyPropertyDescriptor descriptor = DependencyPropertyDescriptor.FromProperty(ListView.ItemsSourceProperty, typeof(ListView));
            descriptor.AddValueChanged(lv, new EventHandler(ItemsSourceChanged));
        }

        private static void ItemsSourceChanged(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            IEnumerable its = lv.ItemsSource;
            IEnumerator itsEnumerator = its?.GetEnumerator();
            
            if (itsEnumerator != null)
            {
                bool hasItems = itsEnumerator.MoveNext();
                if (hasItems)
                {
                    SetUpTheColumns(lv, itsEnumerator.Current);
                }
            }
        }

        private static void SetUpTheColumns(ListView theListView, object firstObject)
        {
            PropertyInfo[] theClassProperties = firstObject.GetType().GetProperties();
            GridView gv = (GridView)theListView.View;

            foreach (PropertyInfo pi in theClassProperties)
            {
                string columnName = pi.Name;

                if (!columnName.Contains("ToolTip"))
                {
                    GridViewColumn grv = new GridViewColumn {Header = columnName.Replace("_", ". ")};
                    FrameworkElementFactory frameworkElementFactory;

                    FrameworkElementFactory frameworkElementFactoryParent = new FrameworkElementFactory(typeof(Grid));
                    frameworkElementFactoryParent.SetValue(FrameworkElement.HorizontalAlignmentProperty,
                        HorizontalAlignment.Stretch);
                    frameworkElementFactoryParent.SetValue(FrameworkElement.VerticalAlignmentProperty,
                        VerticalAlignment.Stretch);
                    frameworkElementFactoryParent.SetValue(Grid.BackgroundProperty, Brushes.Transparent);

                    Binding cellValueBinding = new Binding(columnName);

                    if (pi.Name == "Map")
                    {
                        frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
                        frameworkElementFactory.SetValue(TextBlock.FontWeightProperty, FontWeights.Normal);
                        frameworkElementFactory.SetBinding(TextBlock.TextProperty, cellValueBinding);
                    }
                    else
                    {
                        frameworkElementFactory = new FrameworkElementFactory(typeof(PackIcon));
                        frameworkElementFactory.SetValue(FrameworkElement.MarginProperty,
                            new Thickness(10, 0, 0, 0));
                        frameworkElementFactory.SetBinding(PackIcon.KindProperty, cellValueBinding);

                        Binding toolTipBinding = new Binding("ToolTip_" + columnName.Split('_').Last());
                        // HERE goes note
                        frameworkElementFactoryParent.SetBinding(FrameworkElement.ToolTipProperty, toolTipBinding);
                    }

                    frameworkElementFactoryParent.AppendChild(frameworkElementFactory);

                    grv.CellTemplate = new DataTemplate {VisualTree = frameworkElementFactoryParent};
                    gv.Columns.Add(grv);
                }
            }
        }
    }
}
