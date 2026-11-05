namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class MovimientoStockDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<MovimientoStock>(connectionFactory), IMovimientoStockDao
    {
        private const string TableName = "movimientos_stock";
        private const string PrimaryKeyColumn = "id_movimiento";
        public override int Add(MovimientoStock entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection,
            $"""
            INSERT INTO {TableName} 
                (id_producto, id_tipo_movimiento, id_empleado, id_venta, tipo_stock, cantidad, fecha, motivo) 
            OUTPUT INSERTED.{PrimaryKeyColumn}
            VALUES 
                (@idProducto, @idTipoMovimiento, @idEmpleado, @idVenta, @tipoStock, @cantidad, @fecha, @motivo);
            """);
            AddEntityParameters(command, entity);
            return command.ExecuteNonQuery();
        }
        public override IEnumerable<MovimientoStock> GetAll()
        {
            throw new NotImplementedException();
        }
        public override MovimientoStock? GetById(int id)
        {
            throw new NotImplementedException();
        }
        public override bool Update(MovimientoStock entity)
        {
            throw new NotImplementedException();
        }
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        private static void AddEntityParameters(IDbCommand command, MovimientoStock entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@idProducto", entity.IdProducto),
                ("@idTipoMovimiento", entity.IdTipoMovimiento),
                ("@idEmpleado", entity.IdEmpleado),
                ("@idVenta", entity.IdVenta),
                ("@tipoStock", entity.TipoStock),
                ("@cantidad", entity.Cantidad),
                ("@fecha", entity.Fecha),
                ("@motivo", entity.Motivo)
            ];
            AddParameters(command, parameters);
        }
    }
}