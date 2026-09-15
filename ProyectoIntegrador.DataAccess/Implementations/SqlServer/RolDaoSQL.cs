namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class RolDaoSQL(IConnectionFactory connectionFactory)
        : DaoBase<Rol>(connectionFactory), IRolDao
    {
        protected override string TableName => "roles";
        protected override string PrimaryKeyColumn => "id_rol";
        protected override string SelectColumns => $"{PrimaryKeyColumn}, {InsertColumns}";
        private static string InsertColumns => "nombre, descripcion";
        public override int Insert(Rol entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    ({InsertColumns})
                VALUES
                    (@Nombre, @Descripcion); 
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query, GetParameters(entity));
        }
        public override bool Update(Rol entity)
        {
            var query = $"""
                UPDATE {TableName} 
                SET
                    nombre = @Nombre
                    , descripcion = @Descripcion 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteNonQuery(query, [("@Id", entity.IdRol), .. GetParameters(entity)]) > 0;
        }
        private static (string, object?)[] GetParameters(Rol entity)
        {
            return [
                ("@Nombre", entity.Nombre),
                ("@Descripcion", entity.Descripcion)
            ];
        }
        protected override Rol Mapper(IDataReader reader)
        {
            int descripcionIndex;
            return new Rol
            {
                IdRol = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Descripcion = reader.IsDBNull(descripcionIndex = reader.GetOrdinal("descripcion"))
                    ? null
                    : reader.GetString(descripcionIndex)
            };
        }
    }
}
