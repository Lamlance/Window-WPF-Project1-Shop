using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace WPF_Project1_Shop.Repository
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            _connectionString = "Server(local); Database=TestDB; Integrated Security=true";
        }
        //protected SqlConnection GetConnection()
        //{

        //}
    }
}
