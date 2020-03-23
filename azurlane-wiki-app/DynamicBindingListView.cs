using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace azurlane_wiki_app
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
            IEnumerator itsEnumerator = its.GetEnumerator();
            bool hasItems = itsEnumerator.MoveNext();
            if (hasItems)
            {
                SetUpTheColumns(lv, itsEnumerator.Current);
            }
        }

        private static void SetUpTheColumns(ListView theListView, object firstObject)
        {
            PropertyInfo[] theClassProperties = firstObject.GetType().GetProperties();
            GridView gv = (GridView)theListView.View;

            foreach (PropertyInfo pi in theClassProperties)
            {
                string columnName = pi.Name;
                string value = pi.GetValue(firstObject).ToString();

                GridViewColumn grv = new GridViewColumn { Header = columnName.Replace("_", ". ") };
                FrameworkElementFactory frameworkElementFactory;
                FrameworkElementFactory frameworkElementFactoryParent = new FrameworkElementFactory(typeof(Grid));
                Binding bnd = new Binding(columnName);

                if (pi.Name == "Map")
                {
                    frameworkElementFactory = new FrameworkElementFactory(typeof(TextBlock));
                    frameworkElementFactory.SetBinding(TextBlock.TextProperty, bnd);
                }
                else
                {
                    frameworkElementFactory = new FrameworkElementFactory(typeof(PackIcon));
                    frameworkElementFactory.SetValue(PackIcon.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                    frameworkElementFactory.SetBinding(PackIcon.KindProperty, bnd);
                    
                    frameworkElementFactoryParent.SetValue(FrameworkElement.HorizontalAlignmentProperty,
                        HorizontalAlignment.Stretch);
                }
                frameworkElementFactoryParent.AppendChild(frameworkElementFactory);

                grv.CellTemplate = new DataTemplate { VisualTree = frameworkElementFactory };
                gv.Columns.Add(grv);
            }
        }
    }
}
