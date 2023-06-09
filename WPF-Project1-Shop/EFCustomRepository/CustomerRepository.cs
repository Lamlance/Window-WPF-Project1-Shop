﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
  public class CustomerRepository : IDisposable
  {
    private RailwayContext dbContext;
    public CustomerRepository(RailwayContext railwayContext)
    {
      dbContext = railwayContext;
    }
    public void Dispose()
    {
      dbContext.SaveChanges();
      dbContext.Dispose();
    }

    public Customer AddCustomer(Customer data)
    {
      dbContext.Customers.Add(data);
      return data;
    }


    public Customer UpdateCustomer(Customer data)
    {
      dbContext.Customers.Update(data);
      return data;
    }

    public Customer? RemoveCustomer(Customer data)
    {
      var deletingCustomer = dbContext.Customers
        .Include(o => o.Orders)
        .SingleOrDefault(c => data.Id == c.Id);

      if (deletingCustomer != null)
      {
        foreach(var o in deletingCustomer.Orders)
        {
          using(OrderRepository repository = new OrderRepository(new RailwayContext()))
          {
            repository.DeleteOrder(o);
          }
        };


        dbContext.SaveChanges();

        dbContext.Customers.Remove(deletingCustomer);
        return deletingCustomer;
      }
      return null;
    }

    public IEnumerable<Customer> GetManyCustomers(int page = 1)
    {
      const int itemPerPage = 500;
      return dbContext.Customers
        .Skip(page > 0 ? page - 1 : 0)
        .OrderBy(x => x.Id)
        .Take(itemPerPage);
    }

    public IEnumerable<Customer>? SearchCustomers(string? firstname, string? middlename, string? lastname, string? email, string? phone)
    {
      var result = dbContext.Customers
        .Where(o =>
          (email == null && phone == null && firstname == null && middlename == null && lastname == null) ? true :
          (
            (!(string.IsNullOrWhiteSpace(email) || o.Email == null) && EF.Functions.ILike(o.Email, email)) ||
            (!(string.IsNullOrEmpty(phone) || o.Phone == null) && EF.Functions.ILike(o.Phone, phone)) ||
            (!(string.IsNullOrEmpty(firstname) || o.FirstName == null) && EF.Functions.ILike(o.FirstName, firstname)) ||
            (!(string.IsNullOrEmpty(middlename) || o.MiddleName == null) && EF.Functions.ILike(o.MiddleName, middlename)) ||
            (!(string.IsNullOrEmpty(lastname) || o.LastName == null) && EF.Functions.ILike(o.LastName, lastname))
          )
        )
        .Take(500);

      return result;
    }
  }
}