namespace ProyectoIntegrador.Console.Menus.Cart
{
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class CartMainMenu(Cliente cliente, List<ItemCarrito> carrito)
    {
        private readonly Cliente _cliente = cliente;
        private readonly List<ItemCarrito> _carrito = carrito;
        public void Show()
        {
            var pageSize = 10;
            var pageNumber = 1;

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

                var totalPages = (int)Math.Ceiling((double)_carrito.Count / pageSize);
                if (pageNumber > totalPages)
                    pageNumber = totalPages > 0
                        ? totalPages
                        : 1;

                Render(pageNumber, pageSize, totalPages, out var items);

                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (int.TryParse(input, out var option))
                {
                    if (option == 0)
                        return;

                    int indice = option - ((pageNumber - 1) * pageSize);
                    if (indice > 0 && indice <= items.Count)
                    {
                        EditItem(items[indice - 1]);
                    }
                    else
                    {
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                    }
                }
                else
                {
                    var command = input.Trim().ToUpper();
                    switch (command)
                    {
                        case "F":
                            //decimal total = 0;
                            //foreach (var item in _carrito)
                            //    total += item.PrecioTotal;
                            //var checkoutMenu = new Checkout.CheckoutMainMenu(_cliente, _carrito);
                            //checkoutMenu.Show(total);
                            break;
                        case "P":
                            pageNumber = 1;
                            break;
                        case "U":
                            pageNumber = totalPages > 0 ? totalPages : 1;
                            break;
                        case "S":
                            if (pageNumber < totalPages)
                                pageNumber++;
                            break;
                        case "A":
                            if (pageNumber > 1)
                                pageNumber--;
                            break;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            ConsoleHelper.Messages.Continuar();
                            break;
                    }
                }
            }
        }
        private void Render(int pageNumber, int pageSize, int totalPages, out List<ItemCarrito> items)
        {
            Console.Clear();
            Console.WriteLine($"{"CARRITO DE COMPRAS",-80}[ Página {pageNumber} de {totalPages} ]");
            Console.WriteLine(new string('=', 100));

            items = _carrito.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            decimal total = 0;
            foreach (var item in _carrito)
                total += item.PrecioTotal;

            var indice = (pageNumber - 1) * pageSize;
            foreach (var item in items)
                Console.WriteLine($"[{++indice}] {item.Producto.Nombre} | Cantidad: {item.Cantidad} | P. Unitario: $ {item.Producto.Precio} | Subtotal: $ {item.PrecioTotal}");

            var line = new string('-', 100);
            Console.Write($"""
                {line}
                TOTAL: $ {total}
                {line}
                [P] Primer página  |  [A] Anterior  |  [S] Siguiente  |  [U] Última página
                {line}
                [F] Finalizar compra
                [0] Volver
                {line}
                Seleccione una opción: 
                """);
        }
        private void EditItem(ItemCarrito item)
        {
            Console.Clear();
            Console.Write($"""
                EDITAR ÍTEM: {item.Producto.Nombre}
                ==================================================
                Cantidad actual en carrito: {item.Cantidad}
                Stock disponible en tienda: {item.Producto.Stock}

                Ingrese la nueva cantidad (ingrese 0 para ELIMINAR el ítem): 
                """);

            var qty = Console.ReadLine()?.Trim();
            if (!int.TryParse(qty, out var newQty) || newQty < 0)
            {
                ConsoleHelper.Messages.OpcionNoValida();
                ConsoleHelper.Messages.Continuar();
                return;
            }

            if (newQty == 0)
            {
                _carrito.Remove(item);
                ConsoleHelper.Messages.OK("El ítem ha sido eliminado del carrito");
            }
            else if (newQty > item.Producto.Stock)
            {
                ConsoleHelper.Messages.Error($"La cantidad ingresada supera el stock disponible ({item.Producto.Stock} unidad(es))");
            }
            else
            {
                item.Cantidad = newQty;
                ConsoleHelper.Messages.OK("Cantidad actualizada correctamente");
            }
            ConsoleHelper.Messages.Continuar();
        }
    }
}
