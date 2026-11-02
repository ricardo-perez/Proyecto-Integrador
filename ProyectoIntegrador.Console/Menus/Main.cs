namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Console.Helpers;
    using System;

    internal class Main
    {
        private readonly ClienteMenu _clienteMenu;
        private readonly EmpleadoMenu _empleadoMenu;
        public Main(ClienteMenu clienteMenu, EmpleadoMenu empleadoMenu)
        {
            _clienteMenu = clienteMenu;
            _empleadoMenu = empleadoMenu;
            Configuration();
        }
        internal void Show()
        {
            Console.Title = "SISTEMA DE GESTIÓN";
            while (true)
            {
                Console.Clear();
                var option = ConsoleHelper.Inputs.GetValidInt("""
                MENÚ PRINCIPAL
                ==============
                [1] Cliente
                [2] Empleado
                [3] Acerca de...
                [0] Salir
                Ingrese una opción: 
                """);
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
                var option = ConsoleHelper.Inputs.GetValidInt("""
                    ¿Seguro que desea salir?
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);
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
            catch (Exception ex)
            {
            }
        }
        internal static void About()
        {
            Console.Clear();
            Console.WriteLine($"""
            ACERCA DE
            =========
            {"Sistema:",-20} Sistema de Gestión de Productos y Clientes
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
