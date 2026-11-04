namespace ProyectoIntegrador.DataAccess.Implementations.MsSql
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    internal class VentaDaoMsSql(IConnectionFactory connectionFactory)
        : AdoDaoBase<Venta>(connectionFactory), IVentaDao
    {
        private const string TableName = "ventas";
        private const string PrimaryKeyColumn = "id_venta";
        private const string SelectBase = $"""
            SELECT 
                {PrimaryKeyColumn}, id_cliente, id_medio_de_pago, fecha, subtotal, descuento_total, monto_total
            FROM 
                {TableName}
        """;
        public override int Add(Venta entity)
        {
            throw new NotImplementedException();
        }
        public override IEnumerable<Venta> GetAll()
        {
            var entityList = new List<Venta>();
            using var connection = CreateConnection();
            using var command = CreateCommand(connection, SelectBase);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                entityList.Add(Map(reader));
            return entityList;
        }
        public override Venta? GetById(int id)
        {
            if (id <= 0)
                return null;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
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
        public override bool Update(Venta entity)
        {
            throw new NotImplementedException();
        }
        public override bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public int Add(Venta entity, IDbConnection connection, IDbTransaction transaction)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            ArgumentNullException.ThrowIfNull(connection, nameof(connection));
            ArgumentNullException.ThrowIfNull(transaction, nameof(transaction));

            using var command = CreateCommand(connection, $"""
                INSERT INTO {TableName} 
                    (id_cliente, id_medio_de_pago, fecha, subtotal, descuento_total, monto_total) 
                OUTPUT INSERTED.{PrimaryKeyColumn}
                VALUES 
                    (@idCliente, @idMedioDePago, @fecha, @subtotal, @descuentoTotal, @montoTotal);
            """, transaction: transaction);

            AddEntityParameters(command, entity);

            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }
        private static void AddEntityParameters(IDbCommand command, Venta entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@idCliente", entity.IdCliente),
                ("@idMedioDePago", entity.IdMedioDePago),
                ("@fecha", entity.Fecha),
                ("@subtotal", entity.Subtotal),
                ("@descuentoTotal", entity.DescuentoTotal),
                ("@montoTotal", entity.MontoTotal)
            ];
            AddParameters(command, parameters);
        }
        private static Venta Map(IDataReader reader)
        {
            return new Venta
            {
                IdVenta = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdCliente = reader.GetInt32(reader.GetOrdinal("id_cliente")),
                IdMedioDePago = reader.GetInt32(reader.GetOrdinal("id_medio_de_pago")),
                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                Subtotal = reader.GetDecimal(reader.GetOrdinal("subtotal")),
                DescuentoTotal = reader.GetDecimal(reader.GetOrdinal("descuento_total")),
                MontoTotal = reader.GetDecimal(reader.GetOrdinal("monto_total"))
            };
        }
    }
}
