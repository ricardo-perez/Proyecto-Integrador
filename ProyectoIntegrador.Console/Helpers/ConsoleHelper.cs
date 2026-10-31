namespace ProyectoIntegrador.Console.Helpers
{
    using System;
    internal static class ConsoleHelper
    {
        internal static class Inputs
        {
            internal static int GetValidInt(string message)
            {
                while (true)
                {
                    Console.Write(message);
                    if (int.TryParse(Console.ReadLine(), out int valor) && valor >= 0)
                        return valor;
                    Messages.Error("Ingrese un número válido");
                }
            }
            internal static string? GetValidText(string message, bool isOptional = false)
            {
                while (true)
                {
                    Console.Write(message);
                    string? textValue = Console.ReadLine()?.Trim();
                    if (isOptional && string.IsNullOrWhiteSpace(textValue))
                        return string.Empty;
                    if (!string.IsNullOrWhiteSpace(textValue))
                        return textValue;
                    Messages.Error($"El campo no puede estar vacío");
                }
            }
        }
        internal static class Messages
        {
            internal static void Info(string message)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            internal static void OK(string message)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            internal static void Error(string message)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ERROR: {message}");
                Console.ResetColor();
            }
            internal static void OpcionNoValida()
            {
                Error("Opción no válida");
            }
            internal static void Volviendo()
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Volviendo...");
                Console.ResetColor();
            }
        }
    }
}
