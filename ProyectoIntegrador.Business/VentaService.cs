namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Collections.Generic;
    using System.Transactions;
    public class VentaService(
        IVentaDao ventaDao,
        IDetalleVentaDao detalleVentaDao,
        IMovimientoStockDao movimientoStockDao)
    {
        private readonly IVentaDao _ventaDao = ventaDao;
        private readonly IDetalleVentaDao _detalleVentaDao = detalleVentaDao;
        private readonly IMovimientoStockDao _movimientoStockDao = movimientoStockDao;
        private const int IdTipoMovimientoVenta = 2;
        public Result<int> RegistrarVenta(int idCliente, int idMedioDePago, List<ItemCarrito> carrito, decimal total)
        {
            if (idCliente <= 0)
                return Result<int>.Failure("Cliente no válido", ErrorType.Validation);

            if (idMedioDePago <= 0)
                return Result<int>.Failure("Medio de pago no válido", ErrorType.Validation);

            if (carrito == null || carrito.Count == 0)
                return Result<int>.Failure("El carrito está vacío", ErrorType.Validation);

            using var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var venta = new Venta
                {
                    IdCliente = idCliente,
                    IdMedioDePago = idMedioDePago,
                    Fecha = DateTime.Now,
                    Subtotal = total,
                    DescuentoTotal = 0m,
                    MontoTotal = total
                };

                int idVenta = _ventaDao.Add(venta);
                if (idVenta <= 0)
                    return Result<int>.Failure("No se pudo registrar la venta", ErrorType.Database);

                foreach (var item in carrito)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = idVenta,
                        IdProducto = item.Producto.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.Producto.Precio,
                        Subtotal = item.Producto.Precio * item.Cantidad
                    };

                    _detalleVentaDao.Add(detalle);

                    var movimiento = new MovimientoStock
                    {
                        IdProducto = item.Producto.IdProducto,
                        IdTipoMovimiento = IdTipoMovimientoVenta,
                        IdVenta = idVenta,
                        TipoStock = "ONLINE",
                        Cantidad = -item.Cantidad,
                        Fecha = DateTime.Now,
                        Motivo = $"Venta Online N° {idVenta}"
                    };

                    _movimientoStockDao.Add(movimiento);
                }

                scope.Complete();
                return Result<int>.Success(idVenta);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error al procesar la venta: {ex.Message}", ErrorType.Unexpected);
            }
        }
    }
}