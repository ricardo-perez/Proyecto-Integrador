namespace ProyectoIntegrador.Entities
{
    public class TipoComprobante
    {
        public int IdTipoComprobante { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
