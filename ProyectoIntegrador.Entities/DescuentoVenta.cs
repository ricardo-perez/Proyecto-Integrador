namespace ProyectoIntegrador.Entities
{
    public class DescuentoVenta
    {
        public int IdDescuentoVenta { get; set; }
        public int IdVenta { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal? Porcentaje { get; set; } = 0.00m;
        public decimal MontoDescuento { get; set; }
        public Venta? Venta { get; set; }
    }
}
