
namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IClienteDao : IDao<Cliente>
    {
        Cliente? GetByEmail(string email);
        IEnumerable<Cliente> SearchByEmail(string email);
        Cliente? GetByApellido(string apellido);
        IEnumerable<Cliente> SearchByApellido(string apellido);
    }
}
