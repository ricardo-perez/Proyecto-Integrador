
namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data;
    public class DescuentoVentaSQL(IConnectionFactory connectionFactory) : DaoBase<DescuentoVenta>(connectionFactory), IDescuentoVentaDao
    {
        protected override string TableName => "descuentos_ventas";
        protected override string PrimaryKeyColumn => "id_descuento_venta";
        private string SelectColumns => $"{PrimaryKeyColumn}, {InsertColumns}";
        private string InsertColumns => $"id_venta, concepto, porcentaje, monto_descuento";

        public bool Delete(int id)
        {
            var query = $"""
                DELETE
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteNonQuery(query, ("@Id", id)) > 0;
        }
        public IEnumerable<DescuentoVenta> FindAll()
        {
            var query = $"""
                SELECT
                    {SelectColumns}
                FROM {TableName}
                """;
            return ExecuteList(query);
        }
        public DescuentoVenta? FindById(int id)
        {
            var query = $"""
                SELECT
                    {SelectColumns}
                FROM {TableName} 
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteSingle(query, ("@Id", id));
        }
        public int Insert(DescuentoVenta entity)
        {
            var query = $"""
                INSERT INTO {TableName}
                    ({InsertColumns})
                VALUES
                    (@IdVenta, @Concepto, @Porcentaje, @MontoDescuento);
                SELECT SCOPE_IDENTITY();
                """;
            return ExecuteScalar<int>(query,
                ("@IdVenta", entity.IdVenta),
                ("@Concepto", entity.Concepto),
                ("@Porcentaje", entity.Porcentaje),
                ("@MontoDescuento", entity.MontoDescuento));
        }
        public bool Update(DescuentoVenta entity)
        {
            var query = $"""
                UPDATE {TableName} 
                SET
                    id_venta = @IdVenta
                    , concepto = @Concepto
                    , porcentaje = @Porcentaje
                    , monto_descuento = @MontoDescuento
                WHERE
                    {PrimaryKeyColumn} = @Id
                """;
            return ExecuteNonQuery(query,
                ("@Id", entity.IdDescuentoVenta),
                ("@IdVenta", entity.IdVenta),
                ("@Concepto", entity.Concepto),
                ("@Porcentaje", entity.Porcentaje),
                ("@MontoDescuento", entity.MontoDescuento)) > 0;

        }
        protected override DescuentoVenta Mapper(IDataReader reader)
        {
            int porcentajeIndex;
            return new DescuentoVenta
            {
                IdDescuentoVenta = reader.GetInt32(reader.GetOrdinal(PrimaryKeyColumn)),
                IdVenta = reader.GetInt32(reader.GetOrdinal("id_venta")),
                Concepto = reader.GetString(reader.GetOrdinal("concepto")),
                Porcentaje = reader.IsDBNull(porcentajeIndex = reader.GetOrdinal("porcentaje"))
                    ? null
                    : reader.GetDecimal(porcentajeIndex),
                MontoDescuento = reader.GetDecimal(reader.GetOrdinal("monto_descuento"))
            };
        }
    }
}
