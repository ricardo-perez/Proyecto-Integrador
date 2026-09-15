namespace ProyectoIntegrador.Entities
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; } = 1;
        public decimal PrecioUnitario { get; set; } = 0.00m;
        public decimal Subtotal { get; set; } = 0.00m;
        public Venta? Venta { get; set; }
        public Producto? Producto { get; set; }
    }
}
