using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_Project1_Shop.Model;

namespace WPF_Project1_Shop.Repository
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public bool AuthenticateUser(NetworkCredential credential)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        bool IUserRepository.Add(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        bool IUserRepository.Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        IEnumerable<UserModel> IUserRepository.GetByAll()
        {
            throw new NotImplementedException();
        }

        UserModel IUserRepository.GetById(int id)
        {
            throw new NotImplementedException();
        }

        UserModel IUserRepository.GetByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
