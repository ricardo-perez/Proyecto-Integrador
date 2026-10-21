namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class EmpleadoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Empleado>(connectionFactory), IEmpleadoDao
    {
        private const string TableName = "empleados";
        private const string PrimaryKeyColumn = "id_empleado";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, id_rol, legajo, nombre, apellido, dni, 
                telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo
            FROM 
                {TableName}
            WHERE
                activo = 1
        """;
        public override int Add(Empleado entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@idRol, @legajo, @nombre, @apellido, @dni, @telefono, @direccion, @email, @nombreUsuario, @contrasenia, @imagenRuta, @activo);
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
                        {PrimaryKeyColumn} = @id
                        AND activo = 1
                """);
            AddParameter(command, "@id", id);
            return command.ExecuteNonQuery() > 0;
        }
        public override IEnumerable<Empleado> GetAll()
        {
            var entityList = new List<Empleado>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public Empleado? GetByApellido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND apellido = @apellido
                """);
            AddParameter(command, "@apellido", apellido.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public IEnumerable<Empleado> SearchByApellido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                return [];
            var entityList = new List<Empleado>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND apellido LIKE @apellido
                """);
            AddParameter(command, "@apellido", $"%{apellido.Trim()}%");
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public Empleado? GetByEmail(string email)
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
        public IEnumerable<Empleado> SearchByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return [];
            var entityList = new List<Empleado>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND email LIKE @email
                """);
            AddParameter(command, "@email", $"%{email.Trim()}%");
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override Empleado? GetById(int id)
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
        public Empleado? GetByLegajo(int legajo)
        {
            if (legajo <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {SelectBase}
                        AND legajo = @legajo
                """);
            AddParameter(command, "@legajo", legajo);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(Empleado entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdEmpleado <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        id_rol = @idRol 
                        , legajo = @legajo
                        , nombre = @nombre
                        , apellido = @apellido
                        , dni = @dni
                        , telefono = @telefono
                        , direccion = @direccion
                        , email = @email
                        , nombre_usuario = @nombreUsuario
                        , contrasena = @contrasenia
                        , imagen_ruta = @imagenRuta
                        , activo = @activo
                    WHERE 
                        {PrimaryKeyColumn} = @id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@id", entity.IdEmpleado);
            return command.ExecuteNonQuery() > 0;
        }
        private static Empleado Map(IDataReader reader)
        {
            int telefonoIndex = reader.GetOrdinal("telefono");
            int direccionIndex = reader.GetOrdinal("direccion");
            int emailIndex = reader.GetOrdinal("email");
            int imagenRutaIndex = reader.GetOrdinal("imagen_ruta");
            return new Empleado
            {
                IdEmpleado = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdRol = reader.GetInt32(reader.GetOrdinal("id_rol")),
                Legajo = reader.GetInt32(reader.GetOrdinal("legajo")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                Dni = reader.GetString(reader.GetOrdinal("dni")),
                Telefono = reader.IsDBNull(telefonoIndex)
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex)
                    ? null
                    : reader.GetString(direccionIndex),
                Email = reader.IsDBNull(emailIndex)
                    ? null
                    : reader.GetString(emailIndex),
                NombreUsuario = reader.GetString(reader.GetOrdinal("nombre_usuario")),
                Contrasenia = reader.GetString(reader.GetOrdinal("contrasena")),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex)
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
        private static void AddEntityParameters(IDbCommand command, Empleado entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@idRol", entity.IdRol),
                ("@legajo", entity.Legajo),
                ("@nombre", entity.Nombre),
                ("@apellido", entity.Apellido),
                ("@dni", entity.Dni),
                ("@telefono", entity.Telefono),
                ("@direccion", entity.Direccion),
                ("@email", entity.Email),
                ("@nombreUsuario", entity.NombreUsuario),
                ("@contrasenia", entity.Contrasenia),
                ("@imagenRuta", entity.ImagenRuta),
                ("@activo", entity.Activo)
            ];
            AddParameters(command, parameters);
        }
    }
}
