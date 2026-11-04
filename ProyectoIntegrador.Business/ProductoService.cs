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
                return Result<List<Producto>>.Failure("El número de página debe ser mayor o igual a 1", ErrorType.Validation);

            if (pageSize < 1)
                return Result<List<Producto>>.Failure("El tamaño de página debe ser mayor o igual a 1", ErrorType.Validation);

            try
            {
                var productos = _productoDao.GetPagedWithStock(pageNumber, pageSize);
                return Result<List<Producto>>.Success(productos.ToList());
            }
            catch (DbException ex)
            {
                return Result<List<Producto>>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<List<Producto>>.Failure($"Ocurrió un error inesperado durante la consulta: {ex.Message}", ErrorType.Unexpected);
            }
        }
        public int ObtenerTotalPaginas(int pageSize)
        {
            if (pageSize < 1)
                return 0;
            try
            {
                int totalRegistros = _productoDao.GetTotalCountForCatalog();
                return (int)Math.Ceiling((double)totalRegistros / pageSize);
            }
            catch
            {
                return 0;
            }
        }
        public int ObtenerStock(int id)
        {
            try
            {
                return _productoDao.GetStock(id);
            }
            catch
            {
                return 0;
            }
        }
    }
}
