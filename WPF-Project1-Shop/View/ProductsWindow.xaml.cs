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
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.View
{
    /// <summary>
    /// Interaction logic for ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ObservableCollection<Product> Products { get; set; }

        public ProductsWindow()
        {
            InitializeComponent();

            // Set the DataContext of the view to itself (the Window)
            DataContext = this;
            //Products = new ObservableCollection<Product>
            //{
            //new Product { Id = 1, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 2, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 3, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 4, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 5, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png" ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 6, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png",  ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 7, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", , ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 8, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 9, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 10, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 11, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 12, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 13, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 14, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //new Product { Id = 15, Image = "https://hanoicomputercdn.com/media/product/66888_hacom_macbook_pro_13_7.png", Brand = "APPLE", ProductName = "M2 - ABCDYXUS", Price = 99.99m, Quantity = 2500, },
            //};
            ProductListView.ItemsSource = Products;
        }

        private void ListProductLoaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
