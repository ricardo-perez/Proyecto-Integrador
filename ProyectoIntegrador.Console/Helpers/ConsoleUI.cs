namespace ProyectoIntegrador.Console.Helpers
{
    using System;
    internal static class ConsoleUI
    {
        internal static void InfoMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static void OKMEssage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        internal static void ErrorMEssage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}");
            Console.ResetColor();
        }
        internal static void OpcionNoValida()
        {
            ErrorMEssage("Opción no válida");
        }
        internal static void Volviendo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Volviendo...");
            Console.ResetColor();
        }
    }
}
