namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data.Common;

    public class ProductoService(IProductoDao productoDao)
    {
        private readonly IProductoDao _productoDao = productoDao;
        public Result<List<Producto>> ObtenerPaginaConStock(int pageNumber, int pageSize)
        {
            if (pageNumber < 1)
                return Result<List<Producto>>.Failure("El número de página debe ser mayor o igual a 1", TipoError.Validacion);

            if (pageSize < 1)
                return Result<List<Producto>>.Failure("El tamaño de página debe ser mayor o igual a 1", TipoError.Validacion);

            try
            {
                var productos = _productoDao.GetPagedWithStock(pageNumber, pageSize);
                return Result<List<Producto>>.Success(productos.ToList());
            }
            catch (DbException ex)
            {
                return Result<List<Producto>>.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<List<Producto>>.Failure($"Ocurrió un error inesperado durante la consulta: {ex.Message}", TipoError.Inesperado);
            }
        }
    }
}
