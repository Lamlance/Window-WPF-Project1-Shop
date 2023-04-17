using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Npgsql;
using System.Data;

namespace WPF_Project1_Shop.Repository
{
    public abstract class RepositoryBase
    {
        private readonly string _connectionString;
        public RepositoryBase()
        {
            // _connectionString = "Server(local); Database=TestDB; Integrated Security=true";
            _connectionString = 
                "Host=containers-us-west-181.railway.app;Port=5457;Database=railway;Username=postgres;Password=8hW9GLBvcosfIKxTNVB3";
        }

        public string ConnectionString => _connectionString;

        protected NpgsqlConnection GetConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(ConnectionString);
            return connection;
        }
    }
}
