namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
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
                activo = 1
        """;
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
                        (@Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @CuitCuil, @Contrasena, @Activo);
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
                    UPDATE
                        {TableName} 
                    SET 
                        activo = 0
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                        AND activo = 1
                """);
            AddParameter(command, "@Id", id);
            return command.ExecuteNonQuery() > 0;
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
        public Cliente? GetByApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND apellido = @Apellido
                """);
            AddParameter(command, "@Apellido", apellido.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public IEnumerable<Cliente> SearchByApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                return [];
            var entityList = new List<Cliente>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND apellido LIKE @Apellido
                """);
            AddParameter(command, "@Apellido", $"%{apellido.Trim()}%");
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public Cliente? GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND email = @Email
                """);
            AddParameter(command, "@Email", email.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public IEnumerable<Cliente> SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return [];
            var entityList = new List<Cliente>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND email LIKE @Email
                """);
            AddParameter(command, "@Email", $"%{email.Trim()}%");
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override Cliente? GetById(int id)
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
                        nombre = @Nombre
                        , apellido = @Apellido
                        , dni = @Dni
                        , telefono = @Telefono
                        , direccion = @Direccion
                        , email = @Email
                        , cuit_cuil = @CuitCuil
                        , contrasena = @Contrasena
                        , activo = @Activo                                
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdCliente);
            return command.ExecuteNonQuery() > 0;
        }
        private static void AddEntityParameters(IDbCommand command, Cliente entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@Nombre", entity.Nombre),
                ("@Apellido", entity.Apellido),
                ("@Dni", entity.Dni),
                ("@Telefono", entity.Telefono),
                ("@Direccion", entity.Direccion),
                ("@Email", entity.Email),
                ("@CuitCuil", entity.CuitCuil),
                ("@Contrasena", entity.Contrasena),
                ("@Activo", entity.Activo)
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
                Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}
