﻿using System;
using System.Collections.Generic;

namespace WPF_Project1_Shop.EFModel;

public partial class Category
{
    public long Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}