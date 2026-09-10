namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class RolDaoSQL(IConnectionFactory connectionFactory) : DaoBase(connectionFactory), IRolDao
    {
        protected override string TableName => "roles";
        public bool Delete(int id)
        {
            string query = $"""
                DELETE
                FROM {TableName} 
                WHERE
                    id_rol = @Id
                """;
            return ExecuteNonQuery(query, ("@Id", id)) > 0;
        }
        public IEnumerable<Rol> FindAll()
        {
            string query = $"""
                SELECT
                    id_rol, nombre, descripcion
                FROM {TableName}
                """;

            return ExecuteList(query, Mapper);
        }
        public Rol? FindById(int id)
        {
            string query = $"""
                SELECT
                    id_rol, nombre, descripcion 
                FROM {TableName} 
                WHERE
                    id_rol = @Id
                """;

            return ExecuteSingle(query, Mapper, ("@Id", id));
        }
        public int Insert(Rol entity)
        {
            string query = $"""
                INSERT INTO {TableName}
                    (nombre, descripcion) 
                VALUES
                    (@Nombre, @Descripcion); 
                SELECT SCOPE_IDENTITY();
                """;

            return ExecuteScalar<int>(query,
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion));
        }
        public bool Update(Rol entity)
        {
            string query = $"""
                UPDATE {TableName} 
                SET
                    nombre = @Nombre
                    , descripcion = @Descripcion 
                WHERE
                    id_rol = @Id
                """;

            return ExecuteNonQuery(query,
                ("@Id", entity.IdRol),
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion)) > 0;
        }
        private static Rol Mapper(IDataReader reader)
        {
            int idIndex = reader.GetOrdinal("id_rol");
            int nombreIndex = reader.GetOrdinal("nombre");
            int descripcionIndex = reader.GetOrdinal("descripcion");
            return new Rol
            {
                IdRol = reader.GetInt32(idIndex),
                Nombre = reader.GetString(nombreIndex),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
    }
}
