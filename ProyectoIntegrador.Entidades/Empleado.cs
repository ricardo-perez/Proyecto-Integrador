namespace ProyectoIntegrador.Entidades
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }
        public int IdRol { get; set; }
        public int Legajo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string? ImagenRuta { get; set; }
        public bool Activo { get; set; } = true;
        public Rol? Rol{ get; set; }
    }
}
