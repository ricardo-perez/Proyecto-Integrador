namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class ClienteDaoSQL(IConnectionFactory connectionFactory)
        : DaoBase<Cliente>(connectionFactory), IClienteDao
    {
        protected override string TableName => "clientes";
        protected override string PrimaryKeyColumn => "id_cliente";
        protected override string SelectColumns => $"{PrimaryKeyColumn}, {InsertColumns}";
        private static string InsertColumns => "nombre, apellido, dni, telefono, direccion, email, cuit_cuil, contrasena, activo";
        public override int Insert(Cliente entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    ({InsertColumns})
                VALUES
                    (@Nombre, @Apellido, @Dni, @Telefono, @Direccion, @Email, @CuitCuil, @Contrasena, @Activo);
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query, GetParameters(entity));
        }
        public override bool Update(Cliente entity)
        {
            var query = $"""
                UPDATE {TableName} 
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
                """;
            return ExecuteNonQuery(query, [("@Id", entity.IdCliente), .. GetParameters(entity)]) > 0;
        }
        private static (string, object?)[] GetParameters(Cliente entity)
        {
            return [
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
        }
        protected override Cliente Mapper(IDataReader reader)
        {
            int telefonoIndex;
            int direccionIndex;
            return new Cliente
            {
                IdCliente = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Apellido = reader.GetString(reader.GetOrdinal("apellido")),
                Dni = reader.GetString(reader.GetOrdinal("dni")),
                Telefono = reader.IsDBNull(telefonoIndex = reader.GetOrdinal("telefono"))
                    ? null
                    : reader.GetString(telefonoIndex),
                Direccion = reader.IsDBNull(direccionIndex = reader.GetOrdinal("direccion"))
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
