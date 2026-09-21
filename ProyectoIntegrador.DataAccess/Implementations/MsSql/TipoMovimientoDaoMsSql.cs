namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    internal class TipoMovimientoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<TipoMovimiento>(connectionFactory), ITipoMovimientoDao
    {
        private const string TableName = "tipos_movimiento";
        private const string PrimaryKeyColumn = "id_tipo_movimiento";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, nombre, descripcion
            FROM 
                {TableName}
        """;
        public override int Add(TipoMovimiento entity)
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
        public override IEnumerable<TipoMovimiento> GetAll()
        {
            var entityList = new List<TipoMovimiento>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override TipoMovimiento? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(TipoMovimiento entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdTipoMovimiento <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        nombre = @Nombre
                        , descripcion = @Descripcion
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdTipoMovimiento);
            return command.ExecuteNonQuery() > 0;
        }
        private static void AddEntityParameters(IDbCommand command, TipoMovimiento entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion)
            ];
            AddParameters(command, parameters);
        }
        private static TipoMovimiento Map(IDataReader reader)
        {
            int descripcionIndex = reader.GetOrdinal("descripcion");
            return new TipoMovimiento
            {
                IdTipoMovimiento = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
    }
}
