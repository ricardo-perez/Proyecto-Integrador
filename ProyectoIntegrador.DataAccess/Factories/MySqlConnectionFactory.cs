namespace ProyectoIntegrador.DataAccess.Factories
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using System.Data;
    public class MySqlConnectionFactory : IConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            throw new NotImplementedException();
        }
    }
}
