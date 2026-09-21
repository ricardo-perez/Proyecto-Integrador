namespace ProyectoIntegrador.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? DescripcionLarga { get; set; }
        public string? ImagenRuta { get; set; }
        public decimal Precio { get; set; }
        public bool ActivoParaVenta { get; set; } = false;
        public bool Activo { get; set; } = false;
    }
}
