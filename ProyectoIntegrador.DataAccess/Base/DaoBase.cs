namespace ProyectoIntegrador.DataAccess.Base
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System.Data;
    public abstract class DaoBase(IConnectionFactory connectionFactory)
    {
        protected readonly IConnectionFactory _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
        protected IDbConnection OpenConnection()
        {
            var connection = _connectionFactory.CreateConnection();
            connection.Open();
            return connection;
        }
    }
}
