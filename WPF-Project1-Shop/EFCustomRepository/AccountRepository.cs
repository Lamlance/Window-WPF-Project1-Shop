using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.EFModel;

namespace WPF_Project1_Shop.EFCustomRepository
{
    public class AccountRepository
    {
        private RailwayContext dbContext;
        public AccountRepository(RailwayContext railwayContext)
        {
            dbContext = railwayContext;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public Account? GetAccount(string userName, string encryptedPassword)
        {
            var account = dbContext.Accounts.FirstOrDefault(a => a.UserName == userName && a.Password == encryptedPassword);

            if (account != null)
            {
                return account;
            }
            else {
                return null;
            }
        }
    }
}
