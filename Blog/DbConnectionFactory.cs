using Microsoft.Data.SqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetDbConnection()
        {
            string postgresUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (!String.IsNullOrEmpty(postgresUrl))
            {
                var conn = new NpgsqlConnection(postgresUrl);
                conn.Open();
                return conn;
            }
            else
            {
                var conn = new SqlConnection(_connectionString);
                conn.Open();
                return conn;
            }
        }
    }

    public interface IDbConnectionFactory
    {
        IDbConnection GetDbConnection();
    }
}
