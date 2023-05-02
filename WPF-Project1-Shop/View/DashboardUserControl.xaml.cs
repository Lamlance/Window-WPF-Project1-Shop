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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project1_Shop.ViewModel;

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for DashboardUserControl.xaml
    /// </summary>
    public partial class DashboardUserControl : UserControl
    {

        DashboardViewModel dashboardViewModel;

        public DashboardUserControl()
        {
            InitializeComponent();
            dashboardViewModel = new DashboardViewModel();

            this.ListBestSellerProducts.ItemsSource = dashboardViewModel.TopSellProducts;
            this.ListOfLastestOrders.ItemsSource = dashboardViewModel.RecentOrders;
            this.ListRunningOutProducts.ItemsSource = dashboardViewModel.TopRunningOutProducts;
        }



    }
}
