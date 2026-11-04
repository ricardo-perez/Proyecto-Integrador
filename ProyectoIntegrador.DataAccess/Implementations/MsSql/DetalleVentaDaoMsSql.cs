namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;
    internal class DetalleVentaDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<DetalleVenta>(connectionFactory), IDetalleVentaDao
    {
        private const string TableName = "detalles_ventas";
        private const string PrimaryKeyColumn = "id_detalle_venta";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, id_venta, id_producto, cantidad, precio_unitario, subtotal
            FROM 
                {TableName}
        """;
        public override int Add(DetalleVenta entity)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<DetalleVenta> GetAll()
        {
            var entityList = new List<DetalleVenta>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override DetalleVenta? GetById(int id)
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
        public override bool Update(DetalleVenta entity)
        {
            throw new NotImplementedException();
        }
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(DetalleVenta entity, IDbConnection connection, IDbTransaction transaction)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

            using var command = CreateCommand(connection, $"""
                INSERT INTO {TableName} 
                    (id_venta, id_producto, cantidad, precio_unitario, subtotal) 
                OUTPUT INSERTED.{PrimaryKeyColumn}
                VALUES 
                    (@idVenta, @idProducto, @cantidad, @precioUnitario, @subtotal);
            """, transaction: transaction);

            AddEntityParameters(command, entity);

            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }
        private static void AddEntityParameters(IDbCommand command, DetalleVenta entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@idVenta", entity.IdVenta),
                ("@idProducto", entity.IdProducto),
                ("@cantidad", entity.Cantidad),
                ("@precioUnitario", entity.PrecioUnitario),
                ("@subtotal", entity.Subtotal)
            ];
            AddParameters(command, parameters);
        }

        private static DetalleVenta Map(IDataReader reader)
        {
            return new DetalleVenta
            {
                IdDetalleVenta = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdVenta = reader.GetInt32(reader.GetOrdinal("id_venta")),
                IdProducto = reader.GetInt32(reader.GetOrdinal("id_producto")),
                Cantidad = reader.GetInt32(reader.GetOrdinal("cantidad")),
                PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("precio_unitario")),
                Subtotal = reader.GetDecimal(reader.GetOrdinal("subtotal"))
            };
        }
    }
}
