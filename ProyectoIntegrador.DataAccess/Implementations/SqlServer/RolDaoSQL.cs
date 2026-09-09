namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Collections.Generic;
    public class RolDaoSQL(IConnectionFactory connectionFactory) : DaoBase(connectionFactory), IRolDao
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Rol> FindAll()
        {
            var roles = new List<Rol>();
            const string query = "SELECT id, nombre, descripcion " +
                "FROM Roles";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            using var reader = command.ExecuteReader();
            int idRolIndex = reader.GetOrdinal("id_rol");
            int nombreIndex = reader.GetOrdinal("nombre");
            while (reader.Read())
            {
                roles.Add(new Rol
                {
                    IdRol = reader.GetInt32(idRolIndex),
                    Nombre = reader.GetString(nombreIndex)
                });
            }
            return roles;
        }
        public Rol? FindById(int id)
        {
            const string query = "SELECT id, nombre, descripcion " +
                "FROM Roles " +
                "WHERE id_rol = @Id";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            var parameter = command.CreateParameter();

            parameter.ParameterName = "@Id";
            parameter.Value = id;
            command.Parameters.Add(parameter);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Rol
                {
                    IdRol = reader.GetInt32(reader.GetOrdinal("id_rol")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre"))
                };
            }
            return null;
        }
        public int Insert(Rol entity)
        {
            throw new NotImplementedException();
        }
        public bool Update(Rol entity)
        {
            const string query = "UPDATE Roles " +
                "SET nombre = @Nombre, descripcion = @Descripcion " +
                "WHERE id_rol = @Id";

            using var connection = OpenConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;

            var paramId = command.CreateParameter();
            paramId.ParameterName = "@Id";
            paramId.Value = entity.IdRol;
            command.Parameters.Add(paramId);

            var paramNombre = command.CreateParameter();
            paramNombre.ParameterName = "@Nombre";
            paramNombre.Value = entity.Nombre;
            command.Parameters.Add(paramNombre);

            var paramDescripcion = command.CreateParameter();
            paramDescripcion.ParameterName = "@Descripcion";
            paramDescripcion.Value = (object?)entity.Descripcion ?? DBNull.Value;
            command.Parameters.Add(paramDescripcion);

            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
