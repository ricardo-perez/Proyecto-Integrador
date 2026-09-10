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
        protected IEnumerable<T> ExecuteList<T>(string query, Func<IDataReader, T> mapper, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                AddParameter(command, name, value);
            }

            using var reader = command.ExecuteReader();

            var list = new List<T>();
            while (reader.Read())
            {
                list.Add(mapper(reader));
            }

            return list;
        }
        protected T ExecuteScalar<T>(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                AddParameter(command, name, value);
            }

            var result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                return default!;
            }

            return (T)Convert.ChangeType(result, typeof(T));
        }
        protected T? ExecuteSingle<T>(string query, Func<IDataReader, T> mapper, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                AddParameter(command, name, value);
            }

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return mapper(reader);
            }

            return default;
        }
        protected int ExecuteNonQuery(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                AddParameter(command, name, value);
            }

            return command.ExecuteNonQuery();
        }
    }
}
