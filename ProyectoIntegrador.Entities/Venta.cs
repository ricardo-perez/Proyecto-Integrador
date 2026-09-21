namespace ProyectoIntegrador.Entities
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public int IdMedioDePago { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public decimal Subtotal { get; set; }
        public decimal DescuentoTotal { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
