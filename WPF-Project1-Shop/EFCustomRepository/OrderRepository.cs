using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
    public class OrderRepository : IDisposable
    {
        private RailwayContext dbContext;
        public OrderRepository(RailwayContext railwayContext)
        {
            dbContext = railwayContext;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public IEnumerable<Order> GetOrderAtPage(int page, int itemPerPage = 15)
        {
            return dbContext.Orders
              .OrderBy(o => o.CreatedAt)
              .Skip(page > 0 ? page - 1 : 0)
              .Take(itemPerPage);
        }
    }
}
