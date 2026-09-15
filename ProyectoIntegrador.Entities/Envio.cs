namespace ProyectoIntegrador.Entities
{
    public class Envio
    {
        public int IdEnvio { get; set; }
        public int IdVenta { get; set; }
        public int IdEstadoEnvio { get; set; }
        public string? CodigoSeguimiento { get; set; }
        public DateTime? FechaDespacho { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public Venta? Venta { get; set; }
        public EstadoEnvio? EstadoEnvio { get; set; }
    }
}
