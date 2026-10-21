namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class MedioDePagoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<MedioDePago>(connectionFactory), IMedioDePagoDao
    {
        private const string TableName = "medios_de_pago";
        private const string PrimaryKeyColumn = "id_medio_de_pago";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, nombre, descripcion
            FROM 
                {TableName}
        """;
        public override int Add(MedioDePago entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (nombre, descripcion) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@nombre, @descripcion);
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
                        {PrimaryKeyColumn} = @id
                """);
            AddParameter(command, "@id", id);
            return command.ExecuteNonQuery() > 0;
        }
        public override IEnumerable<MedioDePago> GetAll()
        {
            var entityList = new List<MedioDePago>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override MedioDePago? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddParameter(command, "@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(MedioDePago entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdMedioDePago <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        nombre = @nombre,
                        descripcion = @descripcion
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@id", entity.IdMedioDePago);
            return command.ExecuteNonQuery() > 0;
        }
        private static void AddEntityParameters(IDbCommand command, MedioDePago entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@nombre", entity.Nombre),
                ("@descripcion", entity.Descripcion)
            ];
            AddParameters(command, parameters);
        }
        private static MedioDePago Map(IDataReader reader)
        {
            int descripcionIndex = reader.GetOrdinal("descripcion");
            return new MedioDePago
            {
                IdMedioDePago = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
    }
}
