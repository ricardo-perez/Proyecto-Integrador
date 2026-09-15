namespace ProyectoIntegrador.Entities
{
    public class MedioDePago
    {
        public int IdMedioDePago { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; };
    }
}
