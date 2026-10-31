
namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IClienteDao : IDao<Cliente>
    {
        Cliente? GetByEmail(string email);
        Cliente? GetByDni(string dni);
        Cliente? GetByCuitCuil(string cuitCuil);        
    }
}
