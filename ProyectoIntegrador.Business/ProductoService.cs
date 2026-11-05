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
        public Result<Producto> CrearProducto(Producto producto)
        {
            try
            {
                if (producto == null)
                    return Result<Producto>.Failure("Datos de producto no válidos", ErrorType.Validation);

                producto.Activo = false;
                producto.ActivoParaVenta = false;

                int id = _productoDao.Add(producto);
                return id > 0
                    ? Result<Producto>.Success(producto, "Producto creado correctamente [inactivo y no disponible para venta]")
                    : Result<Producto>.Failure("No se pudo agregar el producto", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<Producto>.Failure($"Error al crear producto: {ex.Message}", ErrorType.Unexpected);
            }
        }
        public Result<Producto> ActualizarProducto(Producto producto)
        {
            try
            {
                Producto? productoExistente = null;

                productoExistente = _productoDao.GetById(producto.IdProducto);
                if (productoExistente == null)
                    return Result<Producto>.Failure("El producto a actualizar no existe", ErrorType.NotFound);

                productoExistente = _productoDao.GetBySku(producto.Sku);
                if (productoExistente != null
                    && productoExistente.IdProducto != producto.IdProducto)
                    return Result<Producto>.Failure("El SKU ya se encuentra registrado en otro producto", ErrorType.Conflict);

                if (_productoDao.Update(producto))
                    return Result<Producto>.Success(producto, "Producto actualizado con éxito");
                return Result<Producto>.Failure("No se pudo actualizar el producto en la base de datos", ErrorType.Database);
            }
            catch (DbException ex)
            {
                return Result<Producto>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<Producto>.Failure($"Ocurrió un error inesperado durante la actualización: {ex.Message}", ErrorType.Unexpected);
            }
        }
        public Result<List<Producto>> ObtenerTodosLosProductos()
        {
            try
            {
                var productos = _productoDao.GetAll();
                return Result<List<Producto>>.Success(productos.ToList());
            }
            catch (Exception ex)
            {
                return Result<List<Producto>>.Failure($"Error al obtener productos: {ex.Message}", ErrorType.Unexpected);
            }
        }
    }
}
