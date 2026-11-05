namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Console.Menus.Cliente;
    using ProyectoIntegrador.Console.Menus.Empleado;
    using System;

    internal class MainMenu
    {
        private readonly ClienteMainMenu _clienteMenu;
        private readonly EmpleadoMainMenu _empleadoMenu;
        public MainMenu(ClienteMainMenu clienteMenu, EmpleadoMainMenu empleadoMenu)
        {
            _clienteMenu = clienteMenu;
            _empleadoMenu = empleadoMenu;
            Configuration();
        }
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                MENÚ PRINCIPAL
                ==============
                [1] Cliente
                [2] Empleado
                [3] Acerca de...
                [0] Salir
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
                        _clienteMenu.Show();
                        break;
                    case 2:
                        _empleadoMenu.Show();
                        break;
                    case 3:
                        About();
                        break;
                    case 0:
                        if (Exit())
                            return;
                        break;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private static bool Exit()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                ¿Seguro que desea salir?
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
                        Console.WriteLine("Saliendo...");
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
        internal static void Configuration()
        {
            Console.Title = "SISTEMA DE GESTIÓN";
            try
            {
                int ancho = 110;
                int alto = 30;
                Console.WindowWidth = ancho;
                Console.WindowHeight = alto;
                Console.BufferWidth = ancho;
                Console.BufferHeight = alto;
            }
            catch (Exception)
            {
            }
        }
        internal static void About()
        {
            Console.Clear();
            Console.WriteLine($"""
            ACERCA DE
            =========
            {"Sistema:",-20} SISTEMA DE GESTIÓN DE PRODUCTOS Y CLIENTES
            {"Tecnología:",-20} .NET Core / C# / ADO.NET / MSSQL
            {"Desarrollador:",-20} Pérez Brivio, Ricardo Luis
            {"Institución:",-20} UTN - FRD - Tecnicatura Universitaria en Programación
            {"Proyecto:",-20} Trabajo Práctico Integrador
            {"Año:",-20} 2026
            """);
            ConsoleHelper.Messages.Continuar();
        }
    }
}
