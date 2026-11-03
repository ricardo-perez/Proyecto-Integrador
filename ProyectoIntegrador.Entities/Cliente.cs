
namespace ProyectoIntegrador.Entities
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CuitCuil { get; set; } = string.Empty;
        public string Contrasenia { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        public override string ToString()
        {
            return $"""
            Nombre:      {this.Nombre}
            Apellido:    {this.Apellido}
            DNI:         {this.Dni}
            Teléfono:    {this.Telefono ?? "N/A"}
            Dirección:   {this.Direccion ?? "N/A"}
            E-Mail:      {this.Email}
            CUIT / CUIL: {this.CuitCuil}
            Contraseña:  ********
            Estado:      {(this.Activo
                ? "Activo"
                : "Inactivo")}
            """;
        }
    }
}
