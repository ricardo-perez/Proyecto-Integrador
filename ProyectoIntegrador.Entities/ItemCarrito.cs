namespace ProyectoIntegrador.Entities
{
    public class ItemCarrito
    {
        public Producto Producto { get; set; } = null!;
        public int Cantidad { get; set; }
        public decimal PrecioTotal => Producto.Precio * Cantidad;
    }
}
