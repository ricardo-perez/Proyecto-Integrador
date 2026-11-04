namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Collections.Generic;
    using System.Data;
    public class EmpleadoDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Empleado>(connectionFactory), IEmpleadoDao
    {
        private const string TableName = "empleados";
        private const string PrimaryKeyColumn = "id_empleado";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, id_rol, legajo, nombre, apellido, dni, 
                telefono, direccion, email, nombre_usuario, contrasenia, imagen_ruta, activo
            FROM 
                {TableName}
            WHERE
                activo = 1
        """;
        public override int Add(Empleado entity)
        {
            throw new NotImplementedException();
        }
        public override Empleado? GetById(int id)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<Empleado> GetAll()
        {
            throw new NotImplementedException();
        }
        public override bool Update(Empleado entity)
        {
            throw new NotImplementedException();
        }
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Empleado? GetByNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection,
                $"""
                {SelectBase} 
                AND nombre_usuario = @nombreUsuario
                """);
            AddParameter(command, "@nombreUsuario", nombreUsuario.Trim());
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader) 
                : null;
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
                Contrasenia = reader.GetString(reader.GetOrdinal("contrasenia")),
                ImagenRuta = reader.IsDBNull(imagenRutaIndex)
                    ? null
                    : reader.GetString(imagenRutaIndex),
                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
            };
        }
    }
}
