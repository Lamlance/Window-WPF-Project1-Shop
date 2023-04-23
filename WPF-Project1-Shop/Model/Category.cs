using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.Model;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
