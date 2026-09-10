namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Collections.Generic;
    using System.Data;
    public class RolDaoSQL(IConnectionFactory connectionFactory) : DaoBase(connectionFactory), IRolDao
    {
        protected override string TableName => "roles";
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Rol> FindAll()
        {
            string query = @$"
                SELECT id_rol, nombre, descripcion
                FROM {TableName}";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();

            var entities = new List<Rol>();
            while (reader.Read())
            {
                entities.Add(RolMapper(reader));
            }
            return entities;
        }
        public Rol? FindById(int id)
        {
            string query = @$"
                SELECT id_rol, nombre, descripcion 
                FROM {TableName} 
                WHERE id_rol = @Id";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            AddParameter(command, "@Id", id);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return RolMapper(reader);
            }
            return null;
        }
        public int Insert(Rol entity)
        {
            string query = @$"
                INSERT INTO {TableName} (nombre, descripcion) 
                VALUES (@Nombre, @Descripcion); 
                SELECT SCOPE_IDENTITY();";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            AddParameter(command, "@Nombre", entity.Nombre);
            AddParameter(command, "@Descripcion", entity.Descripcion);

            var result = command.ExecuteScalar();

            return Convert.ToInt32(result);
        }
        public bool Update(Rol entity)
        {
            string query = @$"
                UPDATE {TableName} 
                SET nombre = @Nombre, descripcion = @Descripcion 
                WHERE id_rol = @Id";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            AddParameter(command, "@Id", entity.IdRol);
            AddParameter(command, "@Nombre", entity.Nombre);
            AddParameter(command, "@Descripcion", entity.Descripcion);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
        private static Rol RolMapper(IDataReader reader)
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
