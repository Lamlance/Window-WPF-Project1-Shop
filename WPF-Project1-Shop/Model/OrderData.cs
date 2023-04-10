using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
  public class OrderData
  {
    private string _name = "Product";
    private DateTime _orderDate = DateTime.UtcNow;
    private DateTime? _deliverDate = null;

    public string Name { get => _name; set => _name = value; }
    public DateTime OrderDate { get => _orderDate; set => _orderDate = value; }
    public DateTime? DeliverDate { get => _deliverDate; set => _deliverDate = value; }
  }
}
