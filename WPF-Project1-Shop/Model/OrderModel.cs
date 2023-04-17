using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public class OrderModel
    {
        public int Id { get; set; }

        public DateOnly OrderDate { get; set; }

        public DateOnly? DeliverDate { get; set; }
    }
}
