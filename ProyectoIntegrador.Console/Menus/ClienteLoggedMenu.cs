namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteLoggedMenu(Cliente cliente, ClienteService clienteService)
    {
        private readonly Cliente _cliente = cliente;
        private readonly ClienteService _clienteService = clienteService;
        internal void Show()
        {
            int option = 0;
            do
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Bienvenid@, {_cliente.Nombre} {_cliente.Apellido} [ID: {_cliente.IdCliente}]");
                Console.ResetColor();
                var menu = """
                TIENDA
                ======
                [1] Navegar catálogo de productos
                [2] Ver carrito de compras
                [3] Mi perfil
                [9] Cerrar sesión
                Ingrese una opción: 
                """;
                option = ConsoleInput.GetPositiveInt(menu);
                switch (option)
                {
                    case 1:
                        Console.WriteLine("PRODUCTOS");
                        break;
                    case 2:
                        Console.WriteLine("CARRITO DE COMPRAS");
                        break;
                    case 3:
                        Perfil();
                        break;
                    case 9:
                        ConsoleUI.InfoMessage("Cerrando sesión...");
                        break;
                    default:
                        ConsoleUI.OpcionNoValida();
                        break;
                }
            } while (option != 9);
        }
        private void Perfil()
        {
            var menu = $"""
            PERFIL
            ======
            [ID: {_cliente.IdCliente}]
            Apellido: {_cliente.Apellido}
            Nombre: {_cliente.Nombre}
            DNI: {_cliente.Dni}
            Teléfono: {_cliente.Telefono}
            Dirección: {_cliente.Direccion}
            E-Mail: {_cliente.Email}
            CUIT / CUIL: {_cliente.CuitCuil}
            Contraseña: ****
            Cuenta activa: {(_cliente.Activo ? "SÍ" : "NO")}

            [1] Modificar datos
            [9] Volver
            Ingrese una opción: 
            """;
            int option = 0;
            do
            {
                option = ConsoleInput.GetPositiveInt(menu);
                switch (option)
                {
                    case 1:
                        Console.WriteLine("MODIFICAR DATOS");
                        break;
                    case 9:
                        ConsoleUI.InfoMessage("Volviendo...");
                        break;
                    default:
                        ConsoleUI.OpcionNoValida();
                        break;
                }
            } while (option != 9);
        }
    }
}
