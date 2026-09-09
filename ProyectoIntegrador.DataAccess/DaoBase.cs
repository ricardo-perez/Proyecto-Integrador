namespace ProyectoIntegrador.DataAccess
{
    public abstract class DaoBase(IConnectionFactory connectionFactory)
    {
        protected readonly IConnectionFactory _connectionFactory = connectionFactory 
            ?? throw new ArgumentNullException(nameof(connectionFactory));
    }
}
