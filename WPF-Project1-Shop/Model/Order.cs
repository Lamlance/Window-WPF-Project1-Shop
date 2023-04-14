using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.Model;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly? DeliverDate { get; set; }
}
