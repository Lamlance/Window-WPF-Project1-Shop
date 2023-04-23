using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
    public class CustomerRepository : IDisposable
    {
        private RailwayContext dbContext;
        public CustomerRepository(RailwayContext railwayContext)
        {
            dbContext = railwayContext;
        }
        public void Dispose()
        {
            dbContext.Dispose();
        }

        public IEnumerable<Customer> GetCustomerAtPage(int page, int itemPerPage = 15)
        {
            return dbContext.Customers
              .Skip(page > 0 ? page - 1 : 0)
              .Take(itemPerPage);
        }

    }
}
