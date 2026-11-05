namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Data.Common;

    public class MovimientoStockService(IMovimientoStockDao movimientoStockDao)
    {
        private readonly IMovimientoStockDao _movimientoStockDao = movimientoStockDao;
        public Result<MovimientoStock> RegistrarMovimientoStock(MovimientoStock movimientoStock)
        {
            try
            {
                if (movimientoStock.IdProducto <= 0)
                    return Result<MovimientoStock>.Failure("Producto no válido", ErrorType.Validation);
                if (movimientoStock.Cantidad == 0)
                    return Result<MovimientoStock>.Failure("La cantidad no puede ser cero", ErrorType.Validation);
                if (string.IsNullOrWhiteSpace(movimientoStock.Motivo))
                    return Result<MovimientoStock>.Failure("Debe indicar un motivo para el movimiento", ErrorType.Validation);

                movimientoStock.IdTipoMovimiento = 1;
                movimientoStock.Fecha = DateTime.Now;

                var idMovimiento = _movimientoStockDao.Add(movimientoStock);
                if (idMovimiento > 0)
                {
                    movimientoStock.IdMovimiento = idMovimiento;
                    return Result<MovimientoStock>.Success(movimientoStock, "Movimiento de stock registrado con éxito");
                }
                return Result<MovimientoStock>.Failure("No se pudo completar el movimiento de stock en la base de datos", ErrorType.Database);
            }
            catch (DbException ex)
            {
                return Result<MovimientoStock>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<MovimientoStock>.Failure($"Ocurrió un error inesperado durante el registro: {ex.Message}", ErrorType.Unexpected);
            }
        }
    }
}
