namespace ProyectoIntegrador.Entities
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
