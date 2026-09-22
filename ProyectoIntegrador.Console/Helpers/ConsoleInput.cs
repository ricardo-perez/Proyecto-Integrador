namespace ProyectoIntegrador.Console.Helpers
{
    using System;
    internal static class ConsoleInput
    {
        internal static int GetPositiveInt(string message)
        {
            int valor;
            Console.Write(message);
            while (!int.TryParse(Console.ReadLine(), out valor) || valor <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Ingrese un número válido");
                Console.ResetColor();
                Console.WriteLine();
                Console.Write(message);
            }
            return valor;
        }
        internal static string? GetValidText(string message, int minText, int maxText, bool isOptional = false)
        {
            var menu = """
            ¿Quiere intentar nuevamente?
            [1] Sí
            [2] No
            Ingrese una opción: 
            """;
            do
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Debe ingresar un valor válido");
                Console.ResetColor();
                int option = 0;
                do
                {
                    option = GetPositiveInt(menu);
                    switch (option)
                    {
                        case 1:
                            // Sale del switch y del do
                            break;
                        case 2:
                            return null;
                        default:
                            ConsoleUI.OpcionNoValida();
                            break;
                    }
                } while (option != 1);
            } while (true);
        }
    }
}
