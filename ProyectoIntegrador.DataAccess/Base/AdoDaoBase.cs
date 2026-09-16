namespace ProyectoIntegrador.DataAccess.Base
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System.Data;
    public abstract class AdoDaoBase<T>(IConnectionFactory connectionFactory)
        : IDao<T> where T : class
    {
        protected readonly IConnectionFactory ConnectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
        protected IDbConnection CreateConnection()
        {
            var connection = ConnectionFactory.CreateConnection();
            connection.Open();
            return connection;
        }
        protected static IDbCommand CreateCommand(
            IDbConnection connection,
            string commandText,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null)
        {
            ArgumentNullException.ThrowIfNull(connection);
            ArgumentException.ThrowIfNullOrWhiteSpace(commandText);
            var command = connection.CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (transaction != null)
                command.Transaction = transaction;
            return command;
        }
        protected static IDbDataParameter AddParameter(
            IDbCommand command,
            string name,
            object? value,
            DbType? dbType = null,
            ParameterDirection direction = ParameterDirection.Input)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = direction;
            if (dbType.HasValue)
                parameter.DbType = dbType.Value;
            command.Parameters.Add(parameter);
            return parameter;
        }
        protected static void AddParameters(
            IDbCommand command,
            IEnumerable<(string Name, object? Value)> parameters)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(parameters);
            foreach (var (name, value) in parameters)
                AddParameter(command, name, value);
        }
        public abstract T? GetById(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract int Add(T entity);
        public abstract bool Update(T entity);
        public abstract bool Delete(int id);
    }
}
