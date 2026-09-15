namespace ProyectoIntegrador.DataAccess.Base
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System.Data;
    public abstract class DaoBase<T>(IConnectionFactory connectionFactory)
        : IDao<T> where T : class
    {
        protected abstract string TableName { get; }
        protected abstract string PrimaryKeyColumn { get; }
        protected abstract T Mapper(IDataReader reader);
        protected abstract string SelectColumns { get; }
        protected readonly IConnectionFactory _connectionFactory = connectionFactory
            ?? throw new ArgumentNullException(nameof(connectionFactory));
        public virtual IEnumerable<T> FindAll()
        {
            var query = $"""
                SELECT
                    {SelectColumns}
                FROM
                    {TableName}
                """;
            return ExecuteList(query);
        }
        public virtual T? FindById(int id)
        {
            var query = $"""
                SELECT
                    {SelectColumns}
                FROM
                    {TableName}
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteSingle(query, ("@Id", id));
        }
        public virtual bool Delete(int id)
        {
            var query = $"""
                DELETE FROM
                    {TableName}
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteNonQuery(query, ("@Id", id)) > 0;
        }
        public abstract int Insert(T entity);
        public abstract bool Update(T entity);
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
        protected IEnumerable<T> ExecuteList(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = CreateCommand(connection, query, parameters);

            using var reader = command.ExecuteReader();

            var list = new List<T>();
            while (reader.Read())
            {
                list.Add(Mapper(reader));
            }

            return list;
        }
        protected TScalar ExecuteScalar<TScalar>(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = CreateCommand(connection, query, parameters);

            var result = command.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                return default!;
            }

            return (TScalar)Convert.ChangeType(result, typeof(TScalar));
        }
        protected T? ExecuteSingle(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = CreateCommand(connection, query, parameters);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return Mapper(reader);
            }

            return default;
        }
        protected int ExecuteNonQuery(string query, params (string Name, object? Value)[] parameters)
        {
            using var connection = OpenConnection();
            using var command = CreateCommand(connection, query, parameters);

            return command.ExecuteNonQuery();
        }
        private static IDbCommand CreateCommand(IDbConnection connection, string query, (string Name, object? Value)[] parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            foreach (var (name, value) in parameters)
            {
                AddParameter(command, name, value);
            }

            return command;
        }
    }
}
