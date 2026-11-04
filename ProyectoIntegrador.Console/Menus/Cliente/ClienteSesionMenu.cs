namespace ProyectoIntegrador.Console.Menus.Cliente
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Console.Menus.Productos;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteSesionMenu(Cliente cliente, ClienteService clienteService, ProductoService productoService)
    {
        private readonly Cliente _cliente = cliente;
        private readonly ClienteService _clienteService = clienteService;
        private readonly ProductoService _productoService = productoService;
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
                        var productosMenu = new ProductosMainMenu(_productoService, _carrito);
                        productosMenu.Show();
                        break;
                    case 2:
                        Cart();
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
        private void Cart()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("""
                    CARRITO DE COMPRAS
                    ==================
                    """);

                if (_carrito.Count == 0)
                {
                    ConsoleHelper.Messages.Info("El carrito de compras está vacío");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }

                decimal total = 0;
                var indice = 0;

                foreach (var item in _carrito)
                {
                    Console.WriteLine($"[{++indice}] {item.Producto.Nombre} | Cantidad: {item.Cantidad} | P. Unitario: $ {item.Producto.Precio} | Total: $ {item.PrecioTotal}");
                    total += item.PrecioTotal;
                }

                var separador = new string('-', 100);
                Console.WriteLine(separador);
                Console.WriteLine($"TOTAL: $ {total}");
                Console.WriteLine(separador);

                Console.Write("""
                    [1] Finalizar compra
                    [2] Editar carrito
                    [0] Volver
                    Seleccione una opción: 
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
                        ConsoleHelper.Messages.Info("CHECKOUT...");
                        ConsoleHelper.Messages.Continuar();
                        break;
                    case 2:
                        ConsoleHelper.Messages.Info("EDITAR CARRITO...");
                        ConsoleHelper.Messages.Continuar();
                        break;
                    case 0:
                        return;
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
