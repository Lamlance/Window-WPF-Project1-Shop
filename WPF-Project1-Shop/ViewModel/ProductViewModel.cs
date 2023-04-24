using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public class ProductViewModel
  {
    ObservableCollection<Product> productsInPage;
    List<Product>? productsSet;

    Product selectedProduct;

    private int _curPage = 1;
    private int _itemPerPage = 15;


    public ProductViewModel()
    {
      productsInPage = new ObservableCollection<EFModel.Product>();
      selectedProduct = new Product();
      Initialize();
    }

    private async Task Initialize()
    {
      await GetManyProducts();
      setPage();
    }

    public void setPage(int page = 1)
    {
      if(productsSet == null)
      {
        return;
      }
      _curPage = page > 0 ? page : 1;
      int start = (page * _itemPerPage) - _itemPerPage;
      int end = Math.Min(start + _itemPerPage, productsSet.Count());
      productsInPage.Clear();
      for (int i = start; i < end; i++)
      {
        productsInPage.Add(productsSet.ElementAt(i));
      }
    }

    public async Task GetManyProducts()
    {
      var result = await Task<List<Product>>.Run(() =>
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          return repository.GetManyProducts().ToList();
        }
      });
      productsSet = result;
      
    }

    public void AddProduct(Product data)
    {
      try
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          repository.AddProduct(data);
        }
        productsInPage.Insert(0, data);
        SelectedProduct = data;
      }
      catch(Exception e)
      {
        return ;
      }
    }

    public ObservableCollection<Product> Products { get => productsInPage; }
    public Product SelectedProduct { get => selectedProduct; set => selectedProduct = value; }
  }
}
