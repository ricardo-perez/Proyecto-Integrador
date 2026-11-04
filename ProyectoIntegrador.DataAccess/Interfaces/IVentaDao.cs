namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IVentaDao : IDao<Venta>
    {
        IEnumerable<Venta> GetByClienteId(int id);
    }
}
