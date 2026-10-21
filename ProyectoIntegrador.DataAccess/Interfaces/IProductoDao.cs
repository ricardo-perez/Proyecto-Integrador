namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IProductoDao : IDao<Producto>
    {
        IEnumerable<Producto> GetPagedWithStock(int pageNumber, int pageSize);
    }
}
