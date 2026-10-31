namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.Entities;
    using System.Text.RegularExpressions;
    public static class ClienteValidator
    {
        private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex TelefonoRegex = new(@"^\+?[0-9\s\-()]{7,30}$", RegexOptions.Compiled);
        private static readonly Regex CuitCuilRegex = new(@"^[0-9]{2}-[0-9]{8}-[0-9]{1}$", RegexOptions.Compiled);
        private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,50}$", RegexOptions.Compiled);
        public static bool Email(string email)
        {
            return !(string.IsNullOrWhiteSpace(email)
                || email.Length > 254
                || !EmailRegex.IsMatch(email.Trim()));
        }
        public static bool Dni(string dni)
        {
            return !(string.IsNullOrWhiteSpace(dni)
                || dni.Trim().Length > 15);
        }
        public static bool CuitCuil(string cuitCuil)
        {
            return !(string.IsNullOrWhiteSpace(cuitCuil)
                || !CuitCuilRegex.IsMatch(cuitCuil.Trim()));
        }
        public static bool Nombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;
            if (nombre.Trim().Length > 100)
                return false;
            return true;
        }
        public static bool Apellido(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return true;
            if (nombre.Trim().Length > 100)
                return false;
            return true;
        }
        public static bool Telefono(string? telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return true;
            if (telefono.Trim().Length > 30)
                return false;
            return TelefonoRegex.IsMatch(telefono.Trim());
        }
        public static bool Direccion(string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                return true;
            if (direccion.Trim().Length > 200)
                return false;
            return true;
        }
        public static bool Contrasenia(string contrasenia)
        {
            return !(string.IsNullOrWhiteSpace(contrasenia)
                || !PasswordRegex.IsMatch(contrasenia));
        }
    }
}
