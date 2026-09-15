namespace ProyectoIntegrador.Entities
{
    public class MovimientoStock
    {
        private int _cantidad;
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int IdTipoMovimiento { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdVenta { get; set; }
        public string TipoStock { get; set; } = string.Empty; // 'FISICO' u 'ONLINE'
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value == 0)
                    throw new ArgumentException("La cantidad no puede ser igual a cero.", nameof(value));
                _cantidad = value;
            }
        }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Motivo { get; set; }
        public Producto? Producto { get; set; }
        public TipoMovimiento? TipoMovimiento { get; set; }
        public Empleado? Empleado { get; set; }
        public Venta? Venta { get; set; }
    }
}
