namespace ProyectoIntegrador.Entities
{
    public class Factura
    {
        public int IdFactura { get; set; }
        public int IdVenta { get; set; }
        public int IdTipoComprobante { get; set; }
        public string PuntoVenta { get; set; } = string.Empty;
        public string NumeroFactura { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal Total { get; set; }
    }
}
