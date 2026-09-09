namespace ProyectoIntegrador.AccesoDatos
{
    using ProyectoIntegrador.Entidades;
    using System.Collections.Generic;
    public class RolDaoSQL : DaoBase, IRolDao
    {
        public RolDaoSQL(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Rol> FindAll()
        {
            var roles = new List<Rol>();
            const string query = "SELECT * FROM Roles";
            using var connection = _connectionFactory.CreateConnection();
            using var command = connection.CreateCommand();
            command.CommandText = query;
            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                roles.Add(new Rol
                {
                    IdRol = reader.GetInt32(reader.GetOrdinal("Id")),
                    Nombre = reader.GetString(reader.GetOrdinal("Nombre"))
                });
            }
            return roles;
        }
        public Rol FindById(int id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Rol entity)
        {
            throw new NotImplementedException();
        }
        public void Update(Rol entity)
        {
            throw new NotImplementedException();
        }
    }
}
