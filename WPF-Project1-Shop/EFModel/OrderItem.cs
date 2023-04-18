using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.EFModel;

public partial class OrderItem
{
    public long Id { get; set; }

    public long? ProductId { get; set; }

    public long? OrderId { get; set; }

    public double? Price { get; set; }

    public int? Quantity { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
