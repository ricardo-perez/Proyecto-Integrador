namespace ProyectoIntegrador.AccesoDatos
{
    using System.Data;
    public interface IConnectionFactory
    {
        IDbConnection CrearConexion();
    }
}
