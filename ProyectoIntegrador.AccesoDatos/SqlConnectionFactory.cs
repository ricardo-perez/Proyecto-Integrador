namespace ProyectoIntegrador.AccesoDatos
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Data;
    public class SqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
