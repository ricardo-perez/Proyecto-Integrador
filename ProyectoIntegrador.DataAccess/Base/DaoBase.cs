namespace ProyectoIntegrador.DataAccess.Base
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System.Data;
    public abstract class DaoBase(IConnectionFactory connectionFactory)
    {
        protected abstract string TableName { get; }
        protected readonly IConnectionFactory _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
        protected IDbConnection OpenConnection()
        {
            var connection = _connectionFactory.CreateConnection();
            connection.Open();
            return connection;
        }
        protected static void AddParameter(IDbCommand command, string name, object? value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            command.Parameters.Add(parameter);
        }
    }
}
