namespace ProyectoIntegrador.Console.Menus.Empleado
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using System;
    internal class EmpleadoMainMenu(EmpleadoService empleadoService)
    {
        private readonly EmpleadoService _empleadoService = empleadoService;
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                EMPLEADO
                =======
                [1] Iniciar sesión
                [0] Volver
                Ingrese una opción: 
                """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Login();
                        break;
                    case 0:
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private void Login()
        {
            var cancelKeyword = "0";
            Console.Clear();
            Console.WriteLine("""
            INICIO DE SESIÓN  (Ingrese 0 en cualquier campo para cancelar)
            ==============================================================
            """);
            string? user;
            while (true)
            {

                user = ConsoleHelper.Inputs.GetValidText("Usuario: ", cancelKeyword: cancelKeyword);
                if (user == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }
            string? pass;
            while (true)
            {
                pass = ConsoleHelper.Inputs.GetValidText("Contraseña: ", cancelKeyword: cancelKeyword);
                if (pass == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }
            ConsoleHelper.Messages.Info("Verificando datos...");
            var result = _empleadoService.IniciarSesion(user, pass);
            if (!result.IsSuccess)
            {
                ConsoleHelper.Messages.Error(result.Message);
                ConsoleHelper.Messages.Continuar();
                return;
            }
            var empleadoSesionMenu = new EmpleadoSesionMenu(result.Value!);
            empleadoSesionMenu.Show();
            ConsoleHelper.Messages.Info("Sesión finalizada correctamente");
            ConsoleHelper.Messages.Continuar();
        }
    }
}
