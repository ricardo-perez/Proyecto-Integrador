namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ProductosMenu(ProductoService productoService, List<ItemCarrito> carrito)
    {
        private readonly ProductoService _productoService = productoService;
        private readonly List<ItemCarrito> _carrito = carrito;
        internal void Show()
        {
            var pageSize = 2;
            var pageNumber = 1;
            while (true)
            {
                Console.Clear();

                var totalPages = _productoService.ObtenerTotalPaginas(pageSize);

                Console.WriteLine($"{"PRODUCTOS",-80}[ Página {pageNumber} de {totalPages} ]");
                Console.WriteLine(new string('=', 100));

                var result = _productoService.ObtenerPaginaConStock(pageNumber, pageSize);
                if (!result.IsSuccess || result.Value == null || result.Value.Count == 0)
                {
                    ConsoleHelper.Messages.Info("No hay productos disponibles en el catálogo");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                var productos = result.Value;
                var indice = 0;
                foreach (var producto in productos)
                {
                    Console.WriteLine($"[{++indice}] {producto.Nombre} - $ {producto.Precio} | {producto.Descripcion} | STOCK: {producto.Stock}");
                }

                var separador = new string('-', 100);

                Console.Write($"""
                {separador}
                [P] Primer página  |  [A] Anterior  |  [S] Siguiente  |  [U] Última página  |  [0] Volver
                {separador}
                Seleccione una opción: 
                """);
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
                    if (option > 0 && option <= productos.Count)
                    {
                        var producto = productos[option - 1];
                        Detail(producto);
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
                        case "P":
                            pageNumber = 1;
                            break;
                        case "U":
                            pageNumber = totalPages > 0
                                ? totalPages
                                : 1;
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
        private void Detail(Producto producto)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"""
                    DETALLE DE PRODUCTO
                    ===================
                    SKU: {producto.Sku}
                    Nombre: {producto.Nombre}
                    Precio: $ {producto.Precio}
                    Stock disponible: {producto.Stock}

                    {producto.DescripcionLarga}

                    """);
                if (producto.Stock > 0)
                    Console.WriteLine("[1] Agregar al carrito");

                Console.Write("""
                [0] Volver
                Seleccione una opción: 
                """);

                var input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out var option) || option < 0)
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                switch (option)
                {
                    case 1:
                        if (producto.Stock > 0)
                            AddToCart(producto);
                        else
                        {
                            ConsoleHelper.Messages.Error("El producto no cuenta con stock disponible");
                            ConsoleHelper.Messages.Continuar();
                        }
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
        private void AddToCart(Producto producto)
        {
            while (true)
            {
                var item = _carrito.Find(i => i.Producto.IdProducto == producto.IdProducto);
                int cartQty = item?.Cantidad ?? 0;
                int stockQty = producto.Stock - cartQty;

                Console.Clear();
                Console.Write($"""
                AGREGAR AL CARRITO
                ==================
                Producto: {producto.Nombre} - $ {producto.Precio}
                Descripción: {producto.Descripcion}
                Stock disponible en tienda: {producto.Stock} unidad(es)
                Agregada(s) en el carrito: {cartQty} unidad(es)
                Stock disponible para agregar: {stockQty} unidad(es)
                    
                Ingrese la cantidad a agregar (0 para cancelar): 
                """);

                var input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out var qty) || qty < 0)
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (qty == 0)
                    return;

                if (qty > stockQty)
                {
                    ConsoleHelper.Messages.Error($"La cantidad ingresada supera el stock disponible para agregar ({stockQty} unidad(es))");
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (item != null)
                    item.Cantidad += qty;
                else
                {
                    _carrito.Add(new ItemCarrito
                    {
                        Producto = producto,
                        Cantidad = qty
                    });
                }

                ConsoleHelper.Messages.OK($"Se han agregado {qty} unidad(es) al carrito de compras");
                ConsoleHelper.Messages.Continuar();
                return;
            }
        }
    }
}
