namespace ProyectoIntegrador.Console.Menus.Empleado
{
    using System;
    using ProyectoIntegrador.Console.Helpers;
    internal class EmpleadoMainMenu
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
