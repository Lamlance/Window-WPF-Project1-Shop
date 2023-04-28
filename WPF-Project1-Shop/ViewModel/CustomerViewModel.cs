using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
    class CustomerViewModel
    {
        public enum MODIFY_MODE
        {
            NONE, ADD, EDIT, DELETE
        }


        private ObservableCollection<Customer> customersInPage;
        IEnumerable<Customer>? customersSet;
        MODIFY_MODE _modifyMode = MODIFY_MODE.NONE;
        Dictionary<long, int> idToPos = new Dictionary<long, int>();


        public CustomerViewModel()
        {
            customersInPage = new ObservableCollection<Customer>();
        }

        private int _curPage = 1;
        private int _itemPerPage = 15;
        private bool _isSearching = false;


        public ObservableCollection<Customer> CustomersInPage { get => customersInPage; }


        public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }


        public string GetStatusString()
        {
            return $"IS {_modifyMode}";
        }

        public async Task Initialize()
        {
            await GetManyCustomers();
            SetPage(1);
        }


        public void SetPage(int page)
        {
            if (customersSet == null)
            {
                return;
            }

            _curPage = page > 0 ? page : 1;
            customersInPage.Clear();
            idToPos.Clear();

            //var numberOfPages = Math.Floor( (double)((ordersSet.Count() + _itemPerPage - 1) / _itemPerPage));
            int start = (page * _itemPerPage) - _itemPerPage;
            int end = Math.Min(start + _itemPerPage, customersSet.Count());
            for (int i = start; i < end; i++)
            {
                idToPos.Add(customersSet.ElementAt(i).Id, i);
                customersInPage.Add(customersSet.ElementAt(i));
            }
        }

        public async Task GetManyCustomers()
        {
            var result = await Task<List<Order>>.Run(() =>
            {
                using (EFCustomRepository.CustomerRepository customerRepository = new EFCustomRepository.CustomerRepository(new EFModel.RailwayContext()))
                {
                    return customerRepository.GetManyCustomers().ToList();
                }
            });
            customersSet = result;
        }

        public async Task SearchCustomers(string? firstname, string? middlename, string? lastname, string? phone, string? email)
        {
            if (_isSearching)
            {
                return;
            }
            _isSearching = true;
            var result = await Task<List<Order>?>.Run(() =>
            {
                using (CustomerRepository repository = new CustomerRepository(new RailwayContext()))
                {
                    return repository.SearchCustomers(firstname, middlename, lastname, email, phone)!.ToList();
                }
            });
            customersSet = result;
            SetPage(1);
            _isSearching = false;
        }


        public bool AddCustomer(Customer customer)
        {
            try
            {
                using (EFCustomRepository.CustomerRepository customerRepository = new EFCustomRepository.CustomerRepository(new EFModel.RailwayContext()))
                {
                    customerRepository.AddCustomer(customer);
                }
                customersInPage.Insert(0, customer);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            };
        }
    }
}
