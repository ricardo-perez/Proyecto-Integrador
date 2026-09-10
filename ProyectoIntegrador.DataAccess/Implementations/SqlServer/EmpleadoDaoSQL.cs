namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Data;
    public class EmpleadoDaoSQL(IConnectionFactory connectionFactory) : DaoBase(connectionFactory), IEmpleadoDao
    {
        protected override string TableName => "Empleados";
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Empleado> FindAll()
        {
            string query = @$"
                SELECT id_empleado, id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo
                FROM {TableName}";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();

            var entities = new List<Empleado>();
            while (reader.Read())
            {
                entities.Add(EmpleadoMapper(reader));
            }
            return entities;
        }
        public Empleado? FindById(int id)
        {
            throw new NotImplementedException();
        }
        public int Insert(Empleado entity)
        {
            string query = @$"
                INSERT INTO {TableName} (id_rol, legajo, nombre, apellido, dni, telefono, direccion, email, nombre_usuario, contrasena, imagen_ruta, activo) 
                VALUES (@IdRol, @Legajo, @Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @NombreUsuario, @Contrasena, @ImagenRuta, @Activo);
                SELECT SCOPE_IDENTITY();";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            AddParameter(command, "@IdRol", entity.IdRol);
            AddParameter(command, "@Legajo", entity.Legajo);
            AddParameter(command, "@Nombre", entity.Nombre);
            AddParameter(command, "@Apellido", entity.Apellido);
            AddParameter(command, "@Dni", entity.Dni);
            AddParameter(command, "@Telefono", entity.Telefono);
            AddParameter(command, "@Direccion", entity.Direccion);
            AddParameter(command, "@Email", entity.Email);
            AddParameter(command, "@NombreUsuario", entity.NombreUsuario);
            AddParameter(command, "@Contrasena", entity.Contrasena);
            AddParameter(command, "@ImagenRuta", entity.ImagenRuta);
            AddParameter(command, "@Activo", entity.Activo);

            var result = command.ExecuteScalar();

            return Convert.ToInt32(result);
        }
        public bool Update(Empleado entity)
        {
            throw new NotImplementedException();
        }
        private static Empleado EmpleadoMapper(IDataReader reader)
        {
            int idEmpleadoIndex = reader.GetOrdinal("id_empleado");
            int idRolIndex = reader.GetOrdinal("id_rol");
            int legajoIndex = reader.GetOrdinal("legajo");
            int nombreIndex = reader.GetOrdinal("nombre");
            int apellidoIndex = reader.GetOrdinal("apellido");
            int dniIndex = reader.GetOrdinal("dni");
            int telefonoIndex = reader.GetOrdinal("telefono");
            int direccionIndex = reader.GetOrdinal("direccion");
            int emailIndex = reader.GetOrdinal("email");
            int nombreUsuarioIndex = reader.GetOrdinal("nombre_usuario");
            int contrasenaIndex = reader.GetOrdinal("contrasena");
            int imagenRutaIndex = reader.GetOrdinal("imagen_ruta");
            int activoIndex = reader.GetOrdinal("activo");
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