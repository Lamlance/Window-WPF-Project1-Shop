using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for RibbonWindow.xaml
    /// </summary>
    public partial class RibbonWindow : System.Windows.Window
    {
        public RibbonWindow()
        {
            InitializeComponent();
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var screens = new ObservableCollection<TabItem>()
            {
                new TabItem() { Content = new CustomerUserControl()},
                new TabItem() { Content = new CustomerUserControl()},
                new TabItem() { Content = new CustomerUserControl()},
                new TabItem() { Content = new CustomerUserControl()},
                new TabItem() { Content = new CustomerUserControl()}
            };
            Tabs.ItemsSource = screens;
        }



    }
}
