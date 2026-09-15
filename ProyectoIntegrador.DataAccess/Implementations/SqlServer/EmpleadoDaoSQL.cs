namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class EmpleadoDaoSQL(IConnectionFactory connectionFactory)
        : DaoBase<Empleado>(connectionFactory), IEmpleadoDao
    {
        protected override string TableName => "empleados";
        protected override string PrimaryKeyColumn => "id_empleado";
        protected override string SelectColumns => $"{PrimaryKeyColumn}, {InsertColumns}";
        private static string InsertColumns => "id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo";
        public override int Insert(Empleado entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    ({InsertColumns})
                VALUES
                    (@IdRol, @Legajo, @Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @NombreUsuario, @Contrasena, @ImagenRuta, @Activo);
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query, GetParameters(entity));
        }
        public override bool Update(Empleado entity)
        {
            var query = $"""
                UPDATE {TableName} 
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
                """;
            return ExecuteNonQuery(query, [("@Id", entity.IdEmpleado), .. GetParameters(entity)]) > 0;
        }
        private static (string, object?)[] GetParameters(Empleado entity)
        {
            return [
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
        }
        protected override Empleado Mapper(IDataReader reader)
        {
            int telefonoIndex;
            int direccionIndex;
            int emailIndex;
            int imagenRutaIndex;
            return new Empleado
            {
                IdEmpleado = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdRol = reader.GetInt32(reader.GetOrdinal("id_rol")),
                Legajo = reader.GetInt32(reader.GetOrdinal("legajo")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                Dni = reader.GetString(reader.GetOrdinal("dni")),
                Telefono = reader.IsDBNull(telefonoIndex = reader.GetOrdinal("telefono"))
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex = reader.GetOrdinal("direccion"))
                    ? null
                    : reader.GetString(direccionIndex),
                Email = reader.IsDBNull(emailIndex = reader.GetOrdinal("email"))
                    ? null
                    : reader.GetString(emailIndex),
                NombreUsuario = reader.GetString(reader.GetOrdinal("nombre_usuario")),
                Contrasena = reader.GetString(reader.GetOrdinal("contrasena")),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex = reader.GetOrdinal("imagen_ruta"))
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}