namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Collections.Generic;
    using System.Data;
    internal class EmpleadoDaoMsSql(IConnectionFactory connectionFactory)
                : AdoDaoBase<Empleado>(connectionFactory), IEmpleadoDao
    {
        private const string _tableName = "empleados";
        private const string _primaryKeyColumn = "id_empleado";
        private const string _selectBase = $"""
            SELECT 
                E.{_primaryKeyColumn}, E.id_rol, E.legajo, E.nombre, E.apellido, E.dni, 
                E.telefono, E.direccion, E.email, E.nombre_usuario, E.contrasena, E.imagen_ruta, E.activo,
                R.id_rol AS rol_id_rol, R.nombre AS rol_nombre, R.descripcion AS rol_descripcion
            FROM 
                {_tableName} AS E
            INNER JOIN
                roles AS R ON R.id_rol = E.id_rol
        """;
        public override int Add(Empleado entity)
        {
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {_tableName} 
                        (id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo) 
                    VALUES 
                        (@IdRol, @Legajo, @Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @NombreUsuario, @Contrasena, @ImagenRuta, @Activo);
                    SELECT SCOPE_IDENTITY();
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
                        {_tableName} 
                    WHERE 
                        {_primaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            return command.ExecuteNonQuery() > 0;
        }
        public override IEnumerable<Empleado> GetAll()
        {
            var entityList = new List<Empleado>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {_selectBase}
                """);
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
                    {_selectBase}
                    WHERE 
                        E.apellido LIKE @Apellido
                """);
            AddParameter(command, "@Apellido", $"%{apellido}%");
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public Empleado? GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {_selectBase}
                    WHERE 
                        E.email LIKE @Email
                """);
            AddParameter(command, "@Email", $"%{email}%");
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override Empleado? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    {_selectBase}
                    WHERE 
                        E.{_primaryKeyColumn} = @Id
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
                    {_selectBase}
                    WHERE 
                        E.legajo = @Legajo
                """);
            AddParameter(command, "@Legajo", legajo);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(Empleado entity)
        {
            if (entity.IdEmpleado <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {_tableName}
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
                        {_primaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            return command.ExecuteNonQuery() > 0;
        }
        private static Empleado Map(IDataReader reader)
        {
            int telefonoIndex = reader.GetOrdinal("telefono");
            int direccionIndex = reader.GetOrdinal("direccion");
            int emailIndex = reader.GetOrdinal("email");
            int imagenRutaIndex = reader.GetOrdinal("imagen_ruta");
            int rolDescripcionIndex = reader.GetOrdinal("rol_descripcion");
            return new Empleado
            {
                IdEmpleado = reader.GetInt32(reader.GetOrdinal(_primaryKeyColumn)),
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
                Rol = new Rol
                {
                    IdRol = reader.GetInt32(reader.GetOrdinal("rol_id_rol")),
                    Nombre = reader.GetString(reader.GetOrdinal("rol_nombre")),
                    Descripcion = reader.IsDBNull(rolDescripcionIndex)
                        ? null
                        : reader.GetString(rolDescripcionIndex)
                }
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
            if (entity.IdEmpleado > 0)
                parameters.Insert(0, ("@Id", entity.IdEmpleado));
            AddParameters(command, parameters);
        }
    }
}
