namespace ProyectoIntegrador.Entities
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdMedioDePago { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; } = 0.00m;
        public decimal DescuentoTotal { get; set; } = 0.00m;
        public decimal MontoTotal { get; set; } = 0.00m;
        public Cliente? Cliente { get; set; }
        public MedioDePago? MedioDePago { get; set; }
        public List<DetalleVenta> DetallesVenta { get; set; } = [];
    }
}
