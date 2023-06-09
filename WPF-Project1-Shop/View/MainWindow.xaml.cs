﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Project1_Shop.Auth0Model;
using WPF_Project1_Shop.EFModel;
using WPF_Project1_Shop.ViewModel;

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //ObservableCollection<OrderData> orders = new ObservableCollection<OrderData>();
        private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");
        private static OrdersUserControl ordersUserControl = new OrdersUserControl();
        private static ProductsUserControl productsUserControl = new ProductsUserControl();
        private static CustomerUserControl customerUserControl = new CustomerUserControl();
        private static ReportUserControl reportUserControl = new ReportUserControl();
        private static DashboardUserControl dashboardUserControl = new DashboardUserControl();
        private static CategoryUserControl categoryUserControl = new CategoryUserControl();
        CategoryViewModel _categoryViewModel;
        UserInformation? user;

        public UserInformation? User { get => user; set => user = value; }

        public MainWindow(UserInformation? userInformation)
        {
            if (userInformation != null)
            {
                this.User = new UserInformation()
                {
                    Nickname = userInformation.Nickname,
                    Name = userInformation.Name,
                    Email = userInformation.Email,
                    PricturePath = userInformation.PricturePath
                };
            }

            InitializeComponent();
            _categoryViewModel = CategoryViewModel.NewInstance();

            ReportViewModel.OnFinishChangesInDB += () =>
            {
                this.btnApplyIncomeProfit.IsEnabled = true;
            };


        }

        private void PreviewTxtInputNumberOnly(object sender, TextCompositionEventArgs e)
        {
            e.Handled = _regexNumberOnly.IsMatch(e.Text);
        }
        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            this.datePickerFromOrderFilter.SelectedDate = DateTime.Today;
            this.datePickerToOrderFilter.SelectedDate = DateTime.Today;
            var screens = new ObservableCollection<TabItem>()
        {
            new TabItem() {Content = dashboardUserControl},
            new TabItem(){Content = ordersUserControl },
            new TabItem(){Content = productsUserControl },
            new TabItem(){Content = customerUserControl},
            new TabItem(){Content = reportUserControl},
            new TabItem(){Content = categoryUserControl}
        };
            this.MainTabControl.ItemsSource = screens;
            this.UserInfoContentControl.Content = new LoginUserControl(this.User, () =>
            {
                this.Close();
            });

        }
        private void CategoriesListLoaded(object sender, RoutedEventArgs e)
        {
            this.ListRibbonCategoriesList.ItemsSource = _categoryViewModel.Categories;
        }


        private void CategoryChecked(object sender, RoutedEventArgs e)
        {
            if (sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
            {
                Category category = (Category)((Fluent.CheckBox)sender).Content;
                _categoryViewModel.SelectedCategories.Add(category);
                return;
            }
        }
        private void CategoryUnchecked(object sender, RoutedEventArgs e)
        {
            if (sender is Fluent.CheckBox && ((Fluent.CheckBox)sender).Content is Category)
            {
                Category category = (Category)((Fluent.CheckBox)sender).Content;
                _categoryViewModel.SelectedCategories.Remove(category);
                return;
            }
        }


        private void OrderEditModeChecked(object sender, RoutedEventArgs e)
        {
            ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.EDIT;
        }
        private void OrderAddModeChecked(object sender, RoutedEventArgs e)
        {
            ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.ADD;
        }
        private void OrderDeleteModeChecked(object sender, RoutedEventArgs e)
        {
            ordersUserControl.ModifyMode = OrderViewModel.MODIFY_MODE.DELETE;
        }
        private void SearchOrderBtnClick(object sender, RoutedEventArgs e)
        {
            DateTime? from = menuApplyDateFilterOrder.IsChecked ? this.datePickerFromOrderFilter.SelectedDate : null;
            DateTime? to = menuApplyDateFilterOrder.IsChecked ? this.datePickerToOrderFilter.SelectedDate : null;

            double? fromSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyFromOderFilter.Number) : null;
            double? toSub = this.menuApplySumFilterOrder.IsChecked ? decimal.ToDouble(this.txtMoneyToOrderFilter.Number) : null;

            string? address = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxAddressOrderFilter.Text : null;
            string? email = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxEmailOrderFilter.Text : null;
            string? phone = this.menuApplyCustomerFilterOrder.IsChecked ? this.txtBoxPhoneOrderFilter.Text : null;

            ordersUserControl.SearchOrder(from, to, address, email, phone, fromSub, toSub);
        }


        private void SearchProductBtnClick(object sender, RoutedEventArgs e)
        {
            IEnumerable<Category>? categories = this.menuApplyCategoriesProductFilter.IsChecked ? _categoryViewModel.SelectedCategories : null;
            string? name = this.menuApplyNameProductFilter.IsChecked ? this.txtBoxNameProductFilter.Text : null;
            double? fromPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyFromProductFilter.Number) : null;
            double? toPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyToProductFilter.Number) : null;

            productsUserControl.SearchProduct(categories, fromPrice, toPrice, name);
        }
        private void ProductEditModeChecked(object sender, RoutedEventArgs e)
        {
            productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.EDIT;
        }
        private void ProductAddModeChecked(object sender, RoutedEventArgs e)
        {
            productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.ADD;
        }
        private void ProductDeleteModeChecked(object sender, RoutedEventArgs e)
        {
            productsUserControl.ModifyMode = ProductViewModel.MODIFY_MODE.DELETE;
        }
        private void ImportProductFromAccessBtnClick(object sender, RoutedEventArgs e)
        {
            var userControl = new ImportProductFromAccess();
            userControl.OnDataImported += (p) =>
            {
                productsUserControl.AddManyProduct(p);
            };
            var window = new Window()
            {
                Title = "Import access data",
                Content = userControl
            };
            window.ShowDialog();
        }

        private void CustomerEditModeChecked(object sender, RoutedEventArgs e)
        {
            this.toggleBtnAddCustomer.IsChecked = false;
            this.toggleBtnDeleteCustomer.IsChecked = false;
            customerUserControl.ModifyMode = CustomerViewModel.MODIFY_MODE.EDIT;
        }
        private void CustomerAddModeChecked(object sender, RoutedEventArgs e)
        {
            this.toggleBtnEditCustomer.IsChecked = false;
            this.toggleBtnDeleteCustomer.IsChecked = false;
            customerUserControl.ModifyMode = CustomerViewModel.MODIFY_MODE.ADD;
        }
        private void CustomerDeleteModeChecked(object sender, RoutedEventArgs e)
        {
            this.toggleBtnAddCustomer.IsChecked = false;
            this.toggleBtnEditCustomer.IsChecked = false;
            customerUserControl.ModifyMode = CustomerViewModel.MODIFY_MODE.DELETE;
        }
        private void CustomerModeUnchecked(object sender, RoutedEventArgs e)
        {
            customerUserControl.ModifyMode = CustomerViewModel.MODIFY_MODE.NONE;
        }
        private void SearchCustomerBtnClick(object sender, RoutedEventArgs e)
        {

            string? firstname = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxFirstNameCustomerFilter.Text : null;
            string? middlename = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxMiddleNameCustomerFilter.Text : null;
            string? lastname = this.menuApplyCustomerNameFilter.IsChecked ? this.txtBoxLastNameCustomerFilter.Text : null;

            string? phone = this.menuApplyCustomerInformationFilter.IsChecked ? this.txtBoxPhoneCustomer.Text : null;
            string? email = this.menuApplyCustomerInformationFilter.IsChecked ? this.txtBoxEmailCustomer.Text : null;
            customerUserControl.SearchCustomer(firstname, middlename, lastname, phone, email);

        }


        static ReportViewModel.REPORT_GROUP_MODE[] modes = new ReportViewModel.REPORT_GROUP_MODE[3]
          {
        ReportViewModel.REPORT_GROUP_MODE.DATE,
        ReportViewModel.REPORT_GROUP_MODE.MONTH,
        ReportViewModel.REPORT_GROUP_MODE.YEAR
          };
        private void ApplyIncomProfitnBtnClick(object sender, RoutedEventArgs e)
        {
            if (this.DatePickerFromIncomeProfit.SelectedDate == null
              || this.DatePickerToIncomeProfit.SelectedDate == null
              || this.ComboBoxGroupTypeIncomeProfit.SelectedIndex == -1
              || this.ComboBoxGroupTypeIncomeProfit.SelectedIndex > 2)
            {
                return;
            }
            DateOnly fromDate = DateOnly.FromDateTime((DateTime)this.DatePickerFromIncomeProfit.SelectedDate);
            DateOnly toDate = DateOnly.FromDateTime((DateTime)this.DatePickerToIncomeProfit.SelectedDate);
            var mode = modes[this.ComboBoxGroupTypeIncomeProfit.SelectedIndex];

            this.btnApplyIncomeProfit.IsEnabled = false;
            reportUserControl.GetIncomeAndProfit(fromDate, toDate, mode);
        }
        private void ApplyProductCountBtnClick(object sender, RoutedEventArgs e)
        {
            if (this.DatePickerFromProductCount.SelectedDate == null
              || this.DatePickerToProductCount.SelectedDate == null
              || this.ComboBoxGroupTypeProductCount.SelectedIndex == -1
              || this.ComboBoxGroupTypeProductCount.SelectedIndex > 2)
            {
                return;
            }

            DateOnly fromDate = DateOnly.FromDateTime((DateTime)this.DatePickerFromProductCount.SelectedDate);
            DateOnly toDate = DateOnly.FromDateTime((DateTime)this.DatePickerToProductCount.SelectedDate);
            var mode = modes[this.ComboBoxGroupTypeProductCount.SelectedIndex];

            reportUserControl.GetProductCount(fromDate, toDate, mode);
        }

        private void SelectTabChange(object sender, SelectionChangedEventArgs e)
        {
            this.Ribbon.IsMinimized = this.Ribbon.SelectedTabIndex == 0;
        }

        private void CategoryEditModeChecked(object sender, RoutedEventArgs e)
        {
            categoryUserControl.ModifyMode = CategoryViewModel.MODIFY_MODE.EDIT;
        }

        private void CategoryAddModeChecked(object sender, RoutedEventArgs e)
        {
            categoryUserControl.ModifyMode = CategoryViewModel.MODIFY_MODE.ADD;
        }

        private void CategoryDeleteModeChecked(object sender, RoutedEventArgs e)
        {
            categoryUserControl.ModifyMode = CategoryViewModel.MODIFY_MODE.DELETE;
        }
        private void SearchCategoryBtnClick(object sender, RoutedEventArgs e)
        {
            string? name = this.menuApplyCategoryNameFilter.IsChecked ? this.txtBoxCategoryNameFilter.Text : null;
            categoryUserControl.SearchCategory(name);

            //IEnumerable<Category>? categories = this.menuApplyCategoriesProductFilter.IsChecked ? _categoryViewModel.SelectedCategories : null;
            //string? name = this.menuApplyNameProductFilter.IsChecked ? this.txtBoxNameProductFilter.Text : null;
            //double? fromPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyFromProductFilter.Number) : null;
            //double? toPrice = this.menuApplyPriceProductFilter.IsChecked ? decimal.ToDouble(this.txtMoneyToProductFilter.Number) : null;

        }

        private void ExportReportDataToPDF(object sender, RoutedEventArgs e)
        {
            reportUserControl.ExportReportData();
        }
    }
}
