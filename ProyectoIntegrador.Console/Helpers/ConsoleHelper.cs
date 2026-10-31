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
            internal static string? GetValidText(string message, int minText, int maxText, bool isOptional = false)
            {
                while (true)
                {
                    Console.Write(message);
                    string? textValue = Console.ReadLine()?.Trim();
                    if (isOptional && string.IsNullOrWhiteSpace(textValue))
                        return string.Empty;
                    bool isValidText = !string.IsNullOrWhiteSpace(textValue)
                        && textValue.Length >= minText
                        && textValue.Length <= maxText;
                    if (isValidText)
                        return textValue;
                    Messages.Error($"El texto debe tener entre {minText} y {maxText} caracteres");
                    int option = 0;
                    do
                    {
                        option = GetValidInt("""
                        ¿Quiere intentar nuevamente?
                        [1] Sí
                        [2] No
                        Ingrese una opción: 
                        """);
                        switch (option)
                        {
                            case 1:
                                // Sale del switch y del do
                                break;
                            case 2:
                                return null;
                            default:
                                Messages.OpcionNoValida();
                                break;
                        }
                    } while (option != 1);
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
