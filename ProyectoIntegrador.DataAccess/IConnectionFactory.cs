namespace ProyectoIntegrador.DataAccess
{
    using System.Data;
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
