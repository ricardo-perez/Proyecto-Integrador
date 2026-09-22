namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteMenu(ClienteService clienteBusiness)
    {
        private readonly ClienteService _clienteBusiness = clienteBusiness;
        internal void Show()
        {
            var option = 0;
            do
            {
                var menu = """
                    CLIENTE
                    =======
                    [1] Iniciar sesión
                    [2] Registrarse
                    [9] Volver
                    Ingrese una opción: 
                """;
                option = ConsoleInput.GetPositiveInt(menu);
                switch (option)
                {
                    case 1:
                        InicioSesion();
                        break;
                    case 2:
                        Registro();
                        break;
                    case 9:
                        ConsoleUI.Volviendo();
                        break;
                    default:
                        ConsoleUI.OpcionNoValida();
                        break;
                }
            } while (option != 9);
        }
        private void Registro()
        {
            Console.WriteLine("""
                REGISTRO
                ========
            """);
            string? nombre = ConsoleInput.GetValidText("Nombre: ", 1, 100);
            if (nombre is null)
                return;
            string? apellido = ConsoleInput.GetValidText("Apellido: ", 1, 100);
            if (apellido is null)
                return;
            string? dni = ConsoleInput.GetValidText("DNI: ", 1, 15);
            if (dni is null)
                return;
            string? telefonoInput = ConsoleInput.GetValidText("Teléfono (opcional - Enter para omitir): ", 1, 30, isOptional: true);
            if (telefonoInput is null)
                return;
            string? telefono = string.IsNullOrEmpty(telefonoInput)
                ? null
                : telefonoInput;
            string? direccionInput = ConsoleInput.GetValidText("Dirección (opcional - Enter para omitir): ", 1, 200, isOptional: true);
            if (direccionInput is null)
                return;
            string? direccion = string.IsNullOrEmpty(direccionInput)
                ? null
                : direccionInput;
            string? email = ConsoleInput.GetValidText("E-Mail: ", 1, 254);
            if (email is null)
                return;
            string? cuitCuil = ConsoleInput.GetValidText("CUIT / CUIL: ", 1, 13);
            if (cuitCuil is null)
                return;
            string? contrasena = ConsoleInput.GetValidText("Contraseña: ", 1, 255);
            if (contrasena is null)
                return;
            var cliente = new Cliente()
            {
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Telefono = telefonoInput,
                Direccion = direccionInput,
                Email = email,
                CuitCuil = cuitCuil,
                Contrasena = contrasena,
                Activo = true
            };
            ConsoleUI.InfoMessage("Guardando datos...");
            var result = _clienteBusiness.RegistrarCliente(cliente);
            if (result.IsSuccess)
                ConsoleUI.OKMEssage($"{result.Mensaje} (ID : {result.Value.IdCliente})");
            else
                ConsoleUI.ErrorMEssage(result.Mensaje);
            Console.ResetColor();
        }
        private void InicioSesion()
        {
            Console.WriteLine("""
                INICIO DE SESIÓN
                ================
            """);
            string? email = ConsoleInput.GetValidText("E-Mail: ", 1, 254);
            if (email is null)
                return;
            string? contrasena = ConsoleInput.GetValidText("Contraseña: ", 1, 255);
            if (contrasena is null)
                return;
        }
    }
}
