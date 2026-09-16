namespace ProyectoIntegrador.DataAccess.Factories
{
    using Microsoft.Data.SqlClient;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System;
    using System.Data;
    public class MsSqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        public MsSqlConnectionFactory(string connectionString)
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
