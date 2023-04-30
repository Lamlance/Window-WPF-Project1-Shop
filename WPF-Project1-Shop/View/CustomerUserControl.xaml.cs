using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Project1_Shop.EFModel;
using WPF_Project1_Shop.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for CustomerUserControl.xaml
    /// </summary>
    public partial class CustomerUserControl : UserControl
    {

        private static readonly Regex _regexNumberOnly = new Regex("[^0-9.-]+");
        private CustomerViewModel _customerViewModel = new CustomerViewModel();

        ObservableCollection<string> pageDisplay = new ObservableCollection<string>();

        public CustomerViewModel.MODIFY_MODE ModifyMode { get => _customerViewModel.ModifyMode; set => _customerViewModel.ModifyMode = value; }

        public CustomerUserControl()
        {
            InitializeComponent();
            this.ListCustomer.ItemsSource = _customerViewModel.CustomersInPage;
            _customerViewModel.OnDataSetReset += ResetComboPageBox;

            _customerViewModel.OnDataAdd += (customer) =>
            {
                Task.Run(() =>
                {
                    MessageBox.Show($"Added customer {customer.Id}: {customer.LastName} {customer.MiddleName} {customer.FirstName}");
                });
            };
            _customerViewModel.OnDataUpdate += (customer) =>
            {
                Task.Run(() =>
                {
                    MessageBox.Show($"Updated customer {customer.Id}: {customer.LastName} {customer.MiddleName} {customer.FirstName}");
                });
            };
            _customerViewModel.OnDataRemove += (customer) =>
            {
                Task.Run(() =>
                {
                    MessageBox.Show($"Removed customer {customer.Id}: {customer.LastName} {customer.MiddleName} {customer.FirstName}");
                });
            };
        }

        public void SearchCustomer(string? firstname, string? middlename, string? lastname, string? phone, string? email)
        {
            _customerViewModel.SearchCustomers(firstname, middlename, lastname, phone, email);
        }


        public void CustomerFormBtnClick(object sender, RoutedEventArgs e)
        {
            if (ModifyMode == CustomerViewModel.MODIFY_MODE.NONE)
            {
                MessageBox.Show("Select a modify mode");
                return;
            }
            if (ModifyMode == CustomerViewModel.MODIFY_MODE.ADD)
            {
                Customer customer = new Customer()
                {
                    FirstName = this.txtBoxFirstNameCustomer.Text,
                    MiddleName = this.txtBoxMiddleNameCustomer.Text,
                    LastName = this.txtBoxLastNameCustomer.Text,
                    Phone = this.txtBoxPhone.Text,
                    Email = this.txtBoxEmail.Text,
                    Address = this.txtBoxAddress.Text
                };
                _customerViewModel.AddCustomer(customer);
                return;
            }
            if (ModifyMode == CustomerViewModel.MODIFY_MODE.EDIT && this.ListCustomer.SelectedItem is Customer)
            {
                Customer customer = (Customer)this.ListCustomer.SelectedItem;
                customer.FirstName = this.txtBoxFirstNameCustomer.Text;
                customer.MiddleName = this.txtBoxMiddleNameCustomer.Text;
                customer.LastName = this.txtBoxLastNameCustomer.Text;
                customer.Phone = this.txtBoxPhone.Text;
                customer.Email = this.txtBoxEmail.Text;
                customer.Address = this.txtBoxAddress.Text;
                _customerViewModel.UpdateCustomer(customer);
                return;
            }
            if (ModifyMode == CustomerViewModel.MODIFY_MODE.DELETE && this.ListCustomer.SelectedItem is Customer)
            {
                Customer customer = (Customer)this.ListCustomer.SelectedItem;
                customer.FirstName = this.txtBoxFirstNameCustomer.Text;
                customer.MiddleName = this.txtBoxMiddleNameCustomer.Text;
                customer.LastName = this.txtBoxLastNameCustomer.Text;
                customer.Phone = this.txtBoxPhone.Text;
                customer.Email = this.txtBoxEmail.Text;
                customer.Address = this.txtBoxAddress.Text;
                _customerViewModel.RemoveCustomer(customer);
                return;
            }
        }

        private void CustomerListClick(object sender, MouseButtonEventArgs e)
        {
            if (this.ListCustomer.SelectedItem is Customer)
            {
                this.CustomerModifyForm.DataContext = (Customer)ListCustomer.SelectedItem;
            }
        }

        private void CustomerUserControlLoaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _customerViewModel;
            this.labelStatusText.Content = _customerViewModel.ModifyMode;
            this.CustomerPageComboBox.ItemsSource = pageDisplay;
        }

        public void ResetComboPageBox(int totalPage)
        {
            pageDisplay.Clear();
            for (int i = 0; i < totalPage; i++)
            {
                pageDisplay.Add($"Page: {i + 1} / {totalPage}");
            }
        }

        private void PageComboBoxChange(object sender, SelectionChangedEventArgs e)
        {
            _customerViewModel.SetPage(this.CustomerPageComboBox.SelectedIndex);
        }
    }
}
