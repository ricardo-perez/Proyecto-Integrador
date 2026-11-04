namespace ProyectoIntegrador.Console.Menus.Cliente
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Console.Menus.Cart;
    using ProyectoIntegrador.Console.Menus.Productos;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteSesionMenu(Cliente cliente, ClienteService clienteService, ProductoService productoService, MedioDePagoService medioDePagoService)
    {
        private readonly Cliente _cliente = cliente;
        private readonly ClienteService _clienteService = clienteService;
        private readonly ProductoService _productoService = productoService;
        private readonly MedioDePagoService _medioDePagoService = medioDePagoService;
        private readonly List<ItemCarrito> _carrito = [];
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleHelper.Messages.OK($"Bienvenid@, {_cliente.Nombre} {_cliente.Apellido} [ID: {_cliente.IdCliente}]");
                Console.Write("""
                    TIENDA
                    ======
                    [1] Navegar catálogo de productos
                    [2] Carrito de compras
                    [3] Historial de compras
                    [4] Mi perfil
                    [0] Cerrar sesión
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        var productosMenu = new ProductosMainMenu(_carrito, _productoService);
                        productosMenu.Show();
                        break;
                    case 2:
                        var cartMainMenu = new CartMainMenu(_cliente, _carrito, _medioDePagoService);
                        cartMainMenu.Show();
                        break;
                    case 4:
                        var clientePerfilMenu = new ClientePerfilMenu(_cliente, _clienteService);
                        if (!clientePerfilMenu.Show())
                            return;
                        break;
                    case 0:
                        if (SignOut())
                            return;
                        break;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private static bool SignOut()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                    ¿Está seguro de que desea cerrar la sesión?
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var option))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Cerrando sesión...");
                        Console.ResetColor();
                        ConsoleHelper.Messages.Continuar();
                        return true;
                    case 2:
                        return false;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
    }
}
