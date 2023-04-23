using System;
using System.Collections.Generic;
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

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Get the window associated with the button
            Window window = Window.GetWindow(this);

            // Close the window
            window.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BindablePasswordBox_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        //{

        //}
    }
}
