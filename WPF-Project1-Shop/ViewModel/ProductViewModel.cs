﻿using System;
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


    private int _curPage = 1;
    private int _itemPerPage = 15;

    public ObservableCollection<Product> ProductsInPage { get => productsInPage; set => productsInPage = value; }
    public ProductViewModel()
    {
      productsInPage = new ObservableCollection<EFModel.Product>();
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
      }
      catch(Exception e)
      {
        return ;
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
      if(productsSet != null)
      {
        productsSet.Clear();
      }
      productsSet = result!.ToList();
      setPage(1);
    }

  }
}