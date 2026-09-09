namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
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
