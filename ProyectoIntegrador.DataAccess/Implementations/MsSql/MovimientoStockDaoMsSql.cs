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
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, id_producto, id_tipo_movimiento, id_empleado, id_venta, tipo_stock, cantidad, fecha, motivo
            FROM 
                {TableName}
        """;
        public override int Add(MovimientoStock entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, $"""
                INSERT INTO {TableName} 
                    (id_producto, id_tipo_movimiento, id_empleado, id_venta, tipo_stock, cantidad, fecha, motivo) 
                VALUES 
                    (@idProducto, @idTipoMovimiento, @idEmpleado, @idVenta, @tipoStock, @cantidad, @fecha, @motivo);
            """);
            AddEntityParameters(command, entity);
            return command.ExecuteNonQuery();
        }
        public override IEnumerable<MovimientoStock> GetAll()
        {
            var entityList = new List<MovimientoStock>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override MovimientoStock? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection,
            $"""
                {SelectBase}
               WHERE
                {PrimaryKeyColumn} = @id
            """);
            AddParameter(command, "@id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
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
        private static MovimientoStock Map(IDataReader reader)
        {
            int idEmpleadoIndex = reader.GetOrdinal("id_empleado");
            int idVentaIndex = reader.GetOrdinal("id_venta");
            int motivoIndex = reader.GetOrdinal("motivo");
            return new MovimientoStock
            {
                IdMovimiento = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdProducto = reader.GetInt32(reader.GetOrdinal("id_producto")),
                IdTipoMovimiento = reader.GetInt32(reader.GetOrdinal("id_tipo_movimiento")),
                IdEmpleado = reader.IsDBNull(idEmpleadoIndex)
                    ? null
                    : reader.GetInt32(idEmpleadoIndex),
                IdVenta = reader.IsDBNull(idVentaIndex)
                    ? null
                    : reader.GetInt32(idVentaIndex),
                TipoStock = reader.GetString(reader.GetOrdinal("tipo_stock")),
                Cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                Motivo = reader.IsDBNull(motivoIndex)
                    ? null
                    : reader.GetString(motivoIndex)
            };
        }
    }
}