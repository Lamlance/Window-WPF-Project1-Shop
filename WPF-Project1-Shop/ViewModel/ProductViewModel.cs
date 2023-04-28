using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using WPF_Project1_Shop.EFCustomRepository;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.ViewModel
{
  public delegate void ProductDataSetChanged(int totalPage);
  public class ProductViewModel
  {
    public delegate void ModifyProductCallBackType(Product product);

    public event ModifyProductCallBackType OnDataAdd;
    public event ModifyProductCallBackType OnDataRemove;
    public event ModifyProductCallBackType OnDataUpdate;

    public event ProductDataSetChanged? OnDataSetReset;

    ObservableCollection<Product> productsInPage;
    ObservableCollection<Category> selectedProductCategories;

    Dictionary<long, int> idToPagePos;
    List<Product>? productsSet;

    public enum MODIFY_MODE
    {
      NONE, ADD, EDIT, DELETE
    }
    MODIFY_MODE _modifyMode = MODIFY_MODE.NONE;


    private int _curPage = 1;
    private int _itemPerPage = 15;

    public ObservableCollection<Product> ProductsInPage { get => productsInPage; set => productsInPage = value; }
    public MODIFY_MODE ModifyMode { get => _modifyMode; set => _modifyMode = value; }
    public ObservableCollection<Category> SelectedProductCategories { get => selectedProductCategories; set => selectedProductCategories = value; }
    public int ItemPerPage { get => _itemPerPage; set => _itemPerPage = value; }

    public ProductViewModel()
    {
      productsInPage = new ObservableCollection<EFModel.Product>();
      selectedProductCategories = new ObservableCollection<Category>();
      idToPagePos = new Dictionary<long, int>();
      Initialize();
    }

    private async Task Initialize()
    {
      await GetManyProducts();
      setPage();
      OnDataSetReset?.Invoke((int)Math.Ceiling((double)(productsSet != null ? productsSet.Count() : 0) / _itemPerPage));
    }

    public void setPage(int page = 1)
    {
      if (productsSet == null)
      {
        return;
      }
      _curPage = page > 0 ? page : 1;
      int start = (_curPage * _itemPerPage) - _itemPerPage;
      int end = Math.Min(start + _itemPerPage, productsSet.Count());
      productsInPage.Clear();
      idToPagePos.Clear();
      for (int i = start; i < end; i++)
      {
        productsInPage.Add(productsSet.ElementAt(i));
        idToPagePos.Add(productsSet.ElementAt(i).Id, i);
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
        OnDataAdd?.Invoke(data);
      }
      catch (Exception e)
      {
        string msg = e.Message;
        return;
      }
    }

    public async Task AddManyProduct(List<Product> products)
    {
      var reslut = await Task<List<Product>>.Run(() =>
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          return repository.AddManyProduct(products);
        }
      });
      if(productsSet != null)
      {
        productsSet.AddRange(reslut);
        OnDataAdd.Invoke(reslut[0]);
        OnDataSetReset?.Invoke((int)Math.Ceiling((double)(productsSet != null ? productsSet.Count() : 0) / _itemPerPage));
      }
    }

    public async Task UpdateProduct(Product data)
    {
      var result = await Task<Product?>.Run(() =>
      {
        try
        {
          using (ProductRepository repository = new ProductRepository(new RailwayContext()))
          {
            return repository.UpdateProduct(data);
          }
        }
        catch (Exception)
        {
          return null;
        }
      });

      if(result != null)
      {
        if (idToPagePos.ContainsKey(data.Id))
        {
          int pos = idToPagePos[data.Id];
          productsInPage[pos] = data;
        }
        OnDataUpdate?.Invoke(data);
      }
    }

    public async Task RemoveProduct(Product p)
    {
      var result = await Task<Product?>.Run(() =>
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          return repository.RemoveProduct(p);
        }
      });
      if(result != null)
      {
        if (idToPagePos.ContainsKey(p.Id))
        {
          int pos = idToPagePos[p.Id];
          productsInPage.Remove(p);
          productsSet!.Remove(result);
        }
      }
    }

    public async Task SearchProduct(IEnumerable<Category>? categories, double? from, double? to, string? name)
    {
      var result = await Task<List<Product>?>.Run(() =>
      {
        using (ProductRepository repository = new ProductRepository(new RailwayContext()))
        {
          var products = repository.SearchProduct(categories, from, to, name);
          return products!.ToList();
        }
      });
      if (productsSet != null)
      {
        productsSet.Clear();
      }
      productsSet = result!.ToList();
      setPage(1);
      OnDataSetReset?.Invoke((int)Math.Ceiling((double)(productsSet != null ? productsSet.Count() : 0) / _itemPerPage));
    }

  }
}
