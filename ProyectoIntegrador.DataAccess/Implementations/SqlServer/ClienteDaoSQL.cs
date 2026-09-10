namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Collections.Generic;
    using System.Data;
    public class ClienteDaoSQL(IConnectionFactory connectionFactory) : DaoBase<Cliente>(connectionFactory), IClienteDao
    {
        protected override string TableName { get { return "clientes"; } }
        protected override string PrimaryKeyColumn { get { return "id_cliente"; } }
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
        public IEnumerable<Cliente> FindAll()
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo
                FROM {TableName}
                """;
            return ExecuteList(query);
        }
        public Cliente? FindById(int id)
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteSingle(query, ("@Id", id));
        }
        public int Insert(Cliente entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    (nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo)
                VALUES
                    (@Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @CuitCuil, @Contrasena, @Activo);
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query,
                ("@Nombre", entity.Nombre),
                ("@Apellido", entity.Apellido),
                ("@Dni", entity.Dni),
                ("@Telefono", entity.Telefono),
                ("@Direccion", entity.Direccion),
                ("@Email", entity.Email),
                ("@CuitCuil", entity.CuitCuil),
                ("@Contrasena", entity.Contrasena),
                ("@Activo", entity.Activo));
        }
        public bool Update(Cliente entity)
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
                    id_rol = @Id
                """;
            return ExecuteNonQuery(query,
                ("@Id", entity.IdCliente),
                ("@Nombre", entity.Nombre),
                ("@Apellido", entity.Apellido),
                ("@Dni", entity.Dni),
                ("@Telefono", entity.Telefono),
                ("@Direccion", entity.Direccion),
                ("@Email", entity.Email),
                ("@CuitCuil", entity.CuitCuil),
                ("@Contrasena", entity.Contrasena),
                ("@Activo", entity.Activo)) > 0;
        }
        protected override Cliente Mapper(IDataReader reader)
        {
            var idClienteIndex = reader.GetOrdinal(PrimaryKeyColumn);
            var nombreIndex = reader.GetOrdinal("nombre");
            var apellidoIndex = reader.GetOrdinal("apellido");
            var dniIndex = reader.GetOrdinal("dni");
            var telefonoIndex = reader.GetOrdinal("telefono");
            var direccionIndex = reader.GetOrdinal("direccion");
            var emailIndex = reader.GetOrdinal("email");
            var cuitCuilIndex = reader.GetOrdinal("cuit_cuil");
            var contrasenaIndex = reader.GetOrdinal("contrasena");
            var activoIndex = reader.GetOrdinal("activo");
            return new Cliente
            {
                IdCliente = reader.GetInt32(idClienteIndex),
                Nombre = reader.GetString(nombreIndex),
                Apellido = reader.GetString(apellidoIndex),
                Dni = reader.GetString(dniIndex),
                Telefono = reader.IsDBNull(telefonoIndex)
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex)
                    ? null
                    : reader.GetString(direccionIndex),
                Email = reader.GetString(emailIndex),
                Contrasena = reader.GetString(contrasenaIndex),
                Activo = reader.GetBoolean(activoIndex)
            };
        }
    }
}
