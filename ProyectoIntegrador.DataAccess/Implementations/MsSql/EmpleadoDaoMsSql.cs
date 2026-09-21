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
                        (@IdRol, @Legajo, @Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @NombreUsuario, @Contrasena, @ImagenRuta, @Activo);
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
                        AND apellido = @Apellido
                """);
            AddParameter(command, "@Apellido", apellido.Trim());
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
                        AND apellido LIKE @Apellido
                """);
            AddParameter(command, "@Apellido", $"%{apellido.Trim()}%");
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
                        AND email = @Email
                """);
            AddParameter(command, "@Email", email.Trim());
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
                        AND email LIKE @Email
                """);
            AddParameter(command, "@Email", $"%{email.Trim()}%");
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
                        AND {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
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
                        AND legajo = @Legajo
                """);
            AddParameter(command, "@Legajo", legajo);
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
                        id_rol = @IdRol 
                        , legajo = @Legajo
                        , nombre = @Nombre
                        , apellido = @Apellido
                        , dni = @Dni
                        , telefono = @Telefono
                        , direccion = @Direccion
                        , email = @Email
                        , nombre_usuario = @NombreUsuario
                        , contrasena = @Contrasena
                        , imagen_ruta = @ImagenRuta
                        , activo = @Activo
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdEmpleado);
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
                Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex)
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo")),
                Rol = null
            };
        }
        private static void AddEntityParameters(IDbCommand command, Empleado entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@IdRol", entity.IdRol),
                ("@Legajo", entity.Legajo),
                ("@Nombre", entity.Nombre),
                ("@Apellido", entity.Apellido),
                ("@Dni", entity.Dni),
                ("@Telefono", entity.Telefono),
                ("@Direccion", entity.Direccion),
                ("@Email", entity.Email),
                ("@NombreUsuario", entity.NombreUsuario),
                ("@Contrasena", entity.Contrasena),
                ("@ImagenRuta", entity.ImagenRuta),
                ("@Activo", entity.Activo)
            ];
            AddParameters(command, parameters);
        }
    }
}
