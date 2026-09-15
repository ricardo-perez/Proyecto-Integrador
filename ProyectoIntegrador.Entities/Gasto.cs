namespace ProyectoIntegrador.Entities
{
    public class Gasto
    {
        public int IdGasto { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
    }
}
