namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class EmpleadoDaoSQL(IConnectionFactory connectionFactory) : DaoBase<Empleado>(connectionFactory), IEmpleadoDao
    {
        protected override string TableName { get { return "empleados"; } }
        protected override string PrimaryKeyColumn { get { return "id_empleado"; } }
        public bool Delete(int id)
        {
            var query = $"""
                DELETE
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteNonQuery(query, ("@Id", id)) > 0;
        }
        public IEnumerable<Empleado> FindAll()
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo
                FROM {TableName}
                """;
            return ExecuteList(query);
        }
        public Empleado? FindById(int id)
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteSingle(query, ("@Id", id));
        }
        public int Insert(Empleado entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    (id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo)
                VALUES
                    (@IdRol, @Legajo, @Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @NombreUsuario, @Contrasena, @ImagenRuta, @Activo);
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query,
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
                ("@Activo", entity.Activo));
        }
        public bool Update(Empleado entity)
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
            return ExecuteNonQuery(query,
                ("@Id", entity.IdEmpleado),
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
                ("@Activo", entity.Activo)) > 0;
        }
        protected override Empleado Mapper(IDataReader reader)
        {
            var idEmpleadoIndex = reader.GetOrdinal(PrimaryKeyColumn);
            var idRolIndex = reader.GetOrdinal("id_rol");
            var legajoIndex = reader.GetOrdinal("legajo");
            var nombreIndex = reader.GetOrdinal("nombre");
            var apellidoIndex = reader.GetOrdinal("apellido");
            var dniIndex = reader.GetOrdinal("dni");
            var telefonoIndex = reader.GetOrdinal("telefono");
            var direccionIndex = reader.GetOrdinal("direccion");
            var emailIndex = reader.GetOrdinal("email");
            var nombreUsuarioIndex = reader.GetOrdinal("nombre_usuario");
            var contrasenaIndex = reader.GetOrdinal("contrasena");
            var imagenRutaIndex = reader.GetOrdinal("imagen_ruta");
            var activoIndex = reader.GetOrdinal("activo");
            return new Empleado
            {
                IdEmpleado = reader.GetInt32(idEmpleadoIndex),
                IdRol = reader.GetInt32(idRolIndex),
                Legajo = reader.GetInt32(legajoIndex),
                Nombre = reader.GetString(nombreIndex),
                Apellido = reader.GetString(apellidoIndex),
                Dni = reader.GetString(dniIndex),
                Telefono = reader.IsDBNull(telefonoIndex)
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex)
                    ? null
                    : reader.GetString(direccionIndex),
                Email = reader.IsDBNull(emailIndex)
                    ? null
                    : reader.GetString(emailIndex),
                NombreUsuario = reader.GetString(nombreUsuarioIndex),
                Contrasena = reader.GetString(contrasenaIndex),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex)
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Activo = reader.GetBoolean(activoIndex)
            };
        }
    }
}