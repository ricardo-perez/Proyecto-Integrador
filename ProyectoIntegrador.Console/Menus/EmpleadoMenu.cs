
namespace ProyectoIntegrador.Console.Menus
{
    using System;
    using ProyectoIntegrador.Console.Helpers;
    internal class EmpleadoMenu
    {
        internal void Show()
        {
            Console.Clear();
            Console.WriteLine("""
            LOGIN EMPLEADO
            ==============
            """);
            ConsoleHelper.Messages.Continuar();
        }
    }
}
