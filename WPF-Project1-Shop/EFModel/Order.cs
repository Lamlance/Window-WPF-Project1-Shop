using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.EFModel;

public partial class Order
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public double Subtotal { get; set; }

    public DateOnly CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public string? ShipAddress { get; set; }

    public string? Status { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
