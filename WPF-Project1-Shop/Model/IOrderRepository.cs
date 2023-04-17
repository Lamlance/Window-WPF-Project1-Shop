using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Project1_Shop.Model
{
    public interface IOrderRepository
    {
        bool Add(OrderModel orderModel);
        bool Edit(OrderModel orderModel);
        bool Remove(int id);
        OrderModel GetById(int id);
        OrderModel GetByModelName(string name);
        IEnumerable<OrderModel> GetByAll();
    }
}
