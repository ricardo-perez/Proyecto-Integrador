namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using System.Data;
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
