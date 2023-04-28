using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace WPF_Project1_Shop.EFModel;

public partial class Product : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  public long Id { get; set; }

  public string ProductName { get; set; } = null!;

  public string? Descriptions { get; set; }

  public string ImagePath { get; set; } = null!;

  public double Price { get; set; }

  public int Numbers { get; set; }

  public DateOnly CreatedAt { get; set; }

  public DateOnly? UpdatedAt { get; set; }

  public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

  public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

  public override bool Equals(object? obj)
  {
    if(obj is Product)
    {
      return this.Id == ((Product)obj).Id;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return decimal.ToInt32(this.Id);
  }
}
