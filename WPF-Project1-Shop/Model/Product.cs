using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.Model;

public partial class Product
{
    public int Id { get; set; }

    public string? Image { get; set; }

    public string? Name { get; set; }

    public int Price { get; set; }

    public int Quantity { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}
