namespace ProyectoIntegrador.Console.Menus.Empleado
{
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class EmpleadoSesionMenu(Empleado empleado)
    {
        private readonly Empleado _empleado = empleado;
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.Messages.OK($"Bienvenid@, {_empleado.Nombre} {_empleado.Apellido} [ID: {_empleado.IdEmpleado}]");
                Console.Write("""
                    BACKOFFICE
                    ==========
                    [1] Registrar nuevo producto
                    [2] Navegar y gestionar productos
                    [0] Cerrar sesión
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
                        //AddProduct();
                        break;
                    case 2:
                        //EditProduct();
                        break;
                    case 0:
                        if (SignOut())
                            return;
                        break;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private static bool SignOut()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                    ¿Está seguro de que desea cerrar la sesión?
                    [1] Sí
                    [2] No
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
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Cerrando sesión...");
                        Console.ResetColor();
                        ConsoleHelper.Messages.Continuar();
                        return true;
                    case 2:
                        return false;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
    }
}
