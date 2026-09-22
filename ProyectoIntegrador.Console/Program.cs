var option = 0;
do
{
    var menu = """
    [1] Cliente
    [2] Empleado
    [9] Salir
    Ingrese una opción: 
    """;
    option = GetPositiveInt(menu);
    switch (option)
    {
        case 1:
            Console.WriteLine("LOGIN CLIENTE");
            break;
        case 2:
            Console.WriteLine("LOGIN EMPLEADO");
            break;
        case 9:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Saliendo...");
            Console.ResetColor();
            break;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Opción no válida");
            Console.WriteLine();
            Console.ResetColor();
            break;
    }
} while (option != 9);
static int GetPositiveInt(string mensaje)
{
    int valor;
    Console.Write(mensaje);
    while (!int.TryParse(Console.ReadLine(), out valor) || valor <= 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Error: Ingrese un número mayor a 0.");
        Console.ResetColor();
        Console.WriteLine();
        Console.Write(mensaje);
    }
    return valor;
}