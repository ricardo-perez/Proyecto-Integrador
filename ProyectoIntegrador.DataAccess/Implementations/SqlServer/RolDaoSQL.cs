namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class RolDaoSQL(IConnectionFactory connectionFactory) : DaoBase<Rol>(connectionFactory), IRolDao
    {
        protected override string TableName { get { return "roles"; } }
        protected override string PrimaryKeyColumn { get { return "id_rol"; } }
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
        public IEnumerable<Rol> FindAll()
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, nombre, descripcion
                FROM {TableName}
                """;

            return ExecuteList(query);
        }
        public Rol? FindById(int id)
        {
            var query = $"""
                SELECT
                    {PrimaryKeyColumn}, nombre, descripcion 
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;

            return ExecuteSingle(query, ("@Id", id));
        }
        public int Insert(Rol entity)
        {
            var query = $"""
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
            var query = $"""
                UPDATE {TableName} 
                SET
                    nombre = @Nombre
                    , descripcion = @Descripcion 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;

            return ExecuteNonQuery(query,
                ("@Id", entity.IdRol),
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion)) > 0;
        }
        protected override Rol Mapper(IDataReader reader)
        {
            var idRolIndex = reader.GetOrdinal(PrimaryKeyColumn);
            var nombreIndex = reader.GetOrdinal("nombre");
            var descripcionIndex = reader.GetOrdinal("descripcion");
            return new Rol
            {
                IdRol = reader.GetInt32(idRolIndex),
                Nombre = reader.GetString(nombreIndex),
                Descripcion = reader.IsDBNull(descripcionIndex)
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
    }
}
