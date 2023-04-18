using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.EFModel;

public partial class Product
{
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
}
