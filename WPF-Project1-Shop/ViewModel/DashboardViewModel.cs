using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
    public class DashboardViewModel
    {

        public List<Order>? RecentOrders { get; set; }
        public ObservableCollection<Order> RecentOrdersCollection { get; set; }
        public List<Product>? TopSellProducts { get; set; }
        public ObservableCollection<Product> TopSellProductsCollection { get; set; }
        public List<Product>? TopRunningOutProducts { get; set; }
        public ObservableCollection<Product> TopRunningOutProductsCollection { get; set; }

        public double TotalEarnings { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }

        //public class SaleReport
        //{
        //    public DateTime CreateAt { get; set; }
        //    public double TotalSum { get; set; }
        //}


        public DashboardViewModel()
        {
            Initialize();
        }


        private async Task Initialize()
        {
            RecentOrdersCollection = new ObservableCollection<Order>();
            TopSellProductsCollection = new ObservableCollection<Product>();
            TopRunningOutProductsCollection = new ObservableCollection<Product>();
            await GetOrdersInformation();
            await GetProductsInformation();
            GetData();
        }

        private void GetData()
        {
            if (RecentOrders == null || TopSellProducts == null || TopRunningOutProducts == null) { return; }
            for (int i = 0; i < RecentOrders.Count; i++)
            {
                RecentOrdersCollection.Add(RecentOrders.ElementAt(i));
                TopSellProductsCollection.Add(TopSellProducts.ElementAt(i));
                TopRunningOutProductsCollection.Add(TopRunningOutProducts.ElementAt(i));
            }
        }

        private async Task GetProductsInformation()
        {
            var result = await Task<List<Product>>.Run(() =>
            {
                using (ProductRepository repository = new ProductRepository(new RailwayContext()))
                {
                    return repository.GetManyProducts().ToList();
                }
            });
            
            TotalProducts = result.Where(o => o.Numbers > 0).Count();
            TopSellProducts = result.OrderBy(o => o.OrderItems.Count).Take(5).ToList();
            TopRunningOutProducts = result.Where(o => o.Numbers > 0).OrderBy(o => o.Numbers).Take(5).ToList();
        }

        private async Task GetOrdersInformation()
        {
            var result = await Task<List<Order>>.Run(() =>
            {
                using (OrderRepository repository = new OrderRepository(new RailwayContext()))
                {
                    return repository.GetManyOrders().ToList();
                }
            });

            TotalOrders = result.Count();
            TotalEarnings = result.Where(o => o.Status == "Shipped").Sum(o => o.Subtotal);
            RecentOrders = result.Take(5).ToList();
        }
    }
}
