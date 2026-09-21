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
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    INSERT INTO {TableName} 
                        (id_cliente, id_medio_de_pago, fecha, subtotal, descuento_total, monto_total) 
                    OUTPUT INSERTED.{PrimaryKeyColumn}
                    VALUES 
                        (@IdCliente, @IdMedioDePago, @Fecha, @Subtotal, @DescuentoTotal, @MontoTotal);
                """);
            AddEntityParameters(command, entity);
            var result = command.ExecuteScalar();
            if (result == null || result == DBNull.Value)
                return 0;
            return Convert.ToInt32(result);
        }
        public override bool Delete(int id)
        {
            if (id <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    DELETE FROM
                        {TableName} 
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            return command.ExecuteNonQuery() > 0;
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
                    {PrimaryKeyColumn} = @Id
                """);
            AddParameter(command, "@Id", id);
            using var reader = command.ExecuteReader();
            return reader.Read()
                ? Map(reader)
                : null;
        }
        public override bool Update(Venta entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            if (entity.IdVenta <= 0)
                return false;
            using var connection = CreateConnection();
            using var command = CreateCommand(connection
                , $"""
                    UPDATE
                        {TableName}
                    SET 
                        id_cliente = @IdCliente 
                        , id_medio_de_pago = @IdMedioDePago
                        , fecha = @Fecha
                        , subtotal = @Subtotal
                        , descuento_total = @DescuentoTotal
                        , monto_total = @MontoTotal
                    WHERE 
                        {PrimaryKeyColumn} = @Id
                """);
            AddEntityParameters(command, entity);
            AddParameter(command, "@Id", entity.IdVenta);
            return command.ExecuteNonQuery() > 0;
        }
        private static void AddEntityParameters(IDbCommand command, Venta entity)
        {
            List<(string Name, object? Value)> parameters =
            [
                ("@IdCliente", entity.IdCliente),
                ("@IdMedioDePago", entity.IdMedioDePago),
                ("@Fecha", entity.Fecha),
                ("@Subtotal", entity.Subtotal),
                ("@DescuentoTotal", entity.DescuentoTotal),
                ("@MontoTotal", entity.MontoTotal)
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
                MontoTotal = reader.GetDecimal(reader.GetOrdinal("monto_total")),
                Cliente = null,
                MedioDePago = null,
                DetallesVenta = []
            };
        }
    }
}
