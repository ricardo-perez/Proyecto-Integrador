namespace ProyectoIntegrador.Entities
{
    public class MovimientoStock
    {
        private int _cantidad;
        private string _tipoStock = string.Empty;
        public int IdMovimiento { get; set; }
        public int IdProducto { get; set; }
        public int IdTipoMovimiento { get; set; }
        public int? IdEmpleado { get; set; }
        public int? IdVenta { get; set; }
        public string TipoStock
        {
            get => _tipoStock;
            set
            {
                if(string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El tipo de stock debe ser 'FISICO' u 'ONLINE'", nameof(value));
                value = value.ToUpper();
                if (value != "FISICO" && value != "ONLINE")
                    throw new ArgumentException("El tipo de stock debe ser 'FISICO' u 'ONLINE'", nameof(value));
                _tipoStock = value;
            }
        }
        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value == 0)
                    throw new ArgumentException("La cantidad no puede ser igual a cero", nameof(value));
                _cantidad = value;
            }
        }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string? Motivo { get; set; }
    }
}
