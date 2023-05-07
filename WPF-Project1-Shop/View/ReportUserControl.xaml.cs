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
  /// Interaction logic for ReportUserControl.xaml
  /// </summary>
  public partial class ReportUserControl : UserControl
  {
    private static ReportViewModel reportViewModel = new ReportViewModel();

    public ReportUserControl()
    {
      InitializeComponent();

    }

    private void UserControlLoaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = reportViewModel;
    }

    public void GetIncomeAndProfit(DateOnly fromDate, DateOnly toDate, ReportViewModel.REPORT_GROUP_MODE mode)
    {
      reportViewModel.GetOrderSumGroupByTime(fromDate,toDate,mode);
    }
    public void GetProductCount(DateOnly fromDate, DateOnly toDate, ReportViewModel.REPORT_GROUP_MODE mode)
    {
      reportViewModel.GetOrderItemProductGroupByTime(fromDate,toDate,mode);
    }
  }
}
