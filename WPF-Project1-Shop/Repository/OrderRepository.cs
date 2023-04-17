using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.Repository
{
    public class OrderRepository : RepositoryBase, IOrderRepository
    {
        public bool Add(OrderModel orderModel)
        {
            throw new NotImplementedException();
        }

        public bool Edit(OrderModel orderModel)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrderModel> GetByAll()
        {
            throw new NotImplementedException();
        }

        public OrderModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public OrderModel GetByModelName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
