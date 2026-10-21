namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Console.Helpers;
    using System;
    internal class Main(ClienteMenu clienteMenu)
    {
        private readonly ClienteMenu _clienteMenu = clienteMenu;
        internal void Show()
        {
            var option = 0;
            do
            {
                option = ConsoleHelper.Inputs.GetValidInt("""
                MENÚ PRINCIPAL
                ==============
                [1] Cliente
                [2] Empleado
                [0] Salir
                Ingrese una opción: 
                """);
                switch (option)
                {
                    case 1:
                        _clienteMenu.Show();
                        break;
                    case 2:
                        Console.WriteLine("LOGIN EMPLEADO");
                        break;
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Saliendo...");
                        Console.ResetColor();
                        break;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        break;
                }
            } while (option != 0);
        }
    }
}
