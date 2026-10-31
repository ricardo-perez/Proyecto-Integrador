namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    using System.Net;

    public class ClienteDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Cliente>(connectionFactory), IClienteDao
    {
        private const string TableName = "clientes";
        private const string PrimaryKeyColumn = "id_cliente";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo
            FROM 
                {TableName}
            WHERE
                1 = 1
        """;
        //
        public override int Add(Cliente entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@nombre, @apellido, @dni, @telefono, @direccion, @email, @cuitCuil, @contrasenia, @activo);
                """);
            AddEntityParameters(command, entity);
            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;
            return Convert.ToInt32(result);
        }
        public override Cliente? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND {PrimaryKeyColumn} = @id
                """);
            AddParameter(command, "@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override IEnumerable<Cliente> GetAll()
        {
            var entityList = new List<Cliente>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override bool Update(Cliente entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdCliente <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        nombre = @nombre
                        , apellido = @apellido
                        , dni = @dni
                        , telefono = @telefono
                        , direccion = @direccion
                        , email = @email
                        , cuit_cuil = @cuitCuil
                        , contrasena = @contrasenia
                        , activo = @activo                                
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@id", entity.IdCliente);
            return command.ExecuteNonQuery() > 0;
        }
        public override bool Delete(int id)
        {
            if (id <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName} 
                    SET 
                        activo = 0
                    WHERE 
                        {PrimaryKeyColumn} = @id
                        AND activo = 1
                """);
            AddParameter(command, "@id", id);
            return command.ExecuteNonQuery() > 0;
        }
        //
        public Cliente? GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND email = @email
                """);
            AddParameter(command, "@email", email.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public Cliente? GetByDni(string dni)
        {
            if (string.IsNullOrEmpty(dni))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND dni = @dni
                """);
            AddParameter(command, "@dni", dni.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public Cliente? GetByCuitCuil(string cuitCuil)
        {
            if (string.IsNullOrEmpty(cuitCuil))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND cuit_cuil = @cuitCuil
                """);
            AddParameter(command, "@cuitCuil", cuitCuil.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        //
        private static void AddEntityParameters(IDbCommand command, Cliente entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@nombre", entity.Nombre),
                ("@apellido", entity.Apellido),
                ("@dni", entity.Dni),
                ("@telefono", entity.Telefono),
                ("@direccion", entity.Direccion),
                ("@email", entity.Email),
                ("@cuitCuil", entity.CuitCuil),
                ("@contrasenia", entity.Contrasenia),
                ("@activo", entity.Activo)
            ];
            AddParameters(command, parameters);
        }
        private static Cliente Map(IDataReader reader)
        {
            int telefonoIndex = reader.GetOrdinal("telefono");
            int direccionIndex = reader.GetOrdinal("direccion");
            return new Cliente
            {
                IdCliente = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                Dni = reader.GetString(reader.GetOrdinal("dni")),
                Telefono = reader.IsDBNull(telefonoIndex)
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex)
                    ? null
                    : reader.GetString(direccionIndex),
                Email = reader.GetString(reader.GetOrdinal("email")),
                CuitCuil = reader.GetString(reader.GetOrdinal("cuit_cuil")),
                Contrasenia = reader.GetString(reader.GetOrdinal("contrasena")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }

    }
}
