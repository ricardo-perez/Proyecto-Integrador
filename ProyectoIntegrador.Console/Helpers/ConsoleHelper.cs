namespace ProyectoIntegrador.Console.Helpers
{
    using System;
    internal static class ConsoleHelper
    {
        internal static class Inputs
        {
            internal static string? GetValidText(string message, bool isOptional = false, string cancelKeyword = "")
            {
                while (true)
                {
                    Console.Write(message);
                    string? textValue = Console.ReadLine()?.Trim();
                    if (!string.IsNullOrEmpty(cancelKeyword) 
                        && cancelKeyword.Equals(textValue, StringComparison.OrdinalIgnoreCase))
                        return null;
                    if (isOptional && string.IsNullOrWhiteSpace(textValue))
                        return string.Empty;
                    if (!string.IsNullOrWhiteSpace(textValue))
                        return textValue;
                    Messages.Error("El campo no puede estar vacío");
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
            internal static void Continuar()
            {
                Console.WriteLine("Presione una tecla para continuar...");
                Console.ReadKey(true);
            }
        }
    }
}
