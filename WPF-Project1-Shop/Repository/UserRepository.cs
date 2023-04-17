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
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new Npgsql.NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from users where username=@username and password=@password";
                command.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Text).Value = credential.UserName;
                command.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Text).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }

            return validUser;
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
            bool isQueryCompleted;
            using (var connection = GetConnection())
            using (var command = new Npgsql.NpgsqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update users set password=@password where username=@username";
                command.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Text).Value = userModel.Username;
                command.Parameters.Add("@password", NpgsqlTypes.NpgsqlDbType.Text).Value = userModel.Password;
                
                isQueryCompleted = command.ExecuteScalar() == null ? false : true;
            }

            return isQueryCompleted;
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
