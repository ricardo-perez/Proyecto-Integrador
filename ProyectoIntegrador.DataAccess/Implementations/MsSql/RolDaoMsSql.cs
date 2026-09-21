namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class RolDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Rol>(connectionFactory), IRolDao
    {
        private const string TableName = "roles";
        private const string PrimaryKeyColumn = "id_rol";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, nombre, descripcion
            FROM 
                {TableName}
        """;
        public override int Add(Rol entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (nombre, descripcion) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@Nombre, @Descripcion);
                """);
            AddEntityParameters(command, entity);
            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;
            return Convert.ToInt32(result);
        }
        public override bool Delete(int id)
        {
            if (id <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    DELETE FROM
                        {TableName} 
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            return command.ExecuteNonQuery() > 0;
        }
        public override IEnumerable<Rol> GetAll()
        {
            var entityList = new List<Rol>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override Rol? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(Rol entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdRol <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        nombre = @Nombre,
                        descripcion = @Descripcion
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdRol);
            return command.ExecuteNonQuery() > 0;
        }
        private static Rol Map(IDataReader reader)
        {
            int descripcionIndex = reader.GetOrdinal("descripcion");
            return new Rol
            {
                IdRol = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
        private static void AddEntityParameters(IDbCommand command, Rol entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion)
            ];
            AddParameters(command, parameters);
        }
    }
}
