namespace ProyectoIntegrador.Entities
{
    public class DescuentoVenta
    {
        public int IdDescuentoVenta { get; set; }
        public int IdVenta { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal? Porcentaje { get; set; }
        public decimal MontoDescuento { get; set; }
    }
}
