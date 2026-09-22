namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using System;
    internal class MainMenu(ClienteMenu clienteMenu)
    {
        private readonly ClienteMenu _clienteMenu = clienteMenu;
        internal void Show()
        {
            var option = 0;
            do
            {
                var menu = """
                    [1] Cliente
                    [2] Empleado
                    [9] Salir
                    Ingrese una opción: 
                """;
                option = ConsoleInput.GetPositiveInt(menu);
                switch (option)
                {
                    case 1:
                        _clienteMenu.Show();
                        break;
                    case 2:
                        Console.WriteLine("LOGIN EMPLEADO");
                        break;
                    case 9:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Saliendo...");
                        Console.ResetColor();
                        break;
                    default:
                        ConsoleUI.OpcionNoValida();
                        break;
                }
            } while (option != 9);
        }
    }
}
