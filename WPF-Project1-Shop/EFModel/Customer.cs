using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.EFModel;

public partial class Customer
{

  public long Id { get; set; }

  public string Phone { get; set; } = null!;

  public string FirstName { get; set; } = null!;

  public string LastName { get; set; } = null!;

  public string? MiddleName { get; set; }

  public string? Email { get; set; }

  public string? Address { get; set; }

  public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
