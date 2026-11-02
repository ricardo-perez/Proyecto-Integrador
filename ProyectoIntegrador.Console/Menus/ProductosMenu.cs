namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ProductosMenu(ProductoService productoService)
    {
        private readonly ProductoService _productoService = productoService;
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
                var input = ConsoleHelper.Inputs.GetValidText($"""
                {separador}
                [P] Primer página  |  [A] Anterior  |  [S] Siguiente  |  [U] Última página  |  [0] Volver
                {separador}
                Seleccione una opción: 
                """, isOptional: true);

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (int.TryParse(input.Trim(), out var option))
                {
                    if (option == 0)
                    {
                        ConsoleHelper.Messages.Volviendo();
                        return;
                    }
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
        private static void Detail(Producto producto)
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

                var option = ConsoleHelper.Inputs.GetValidInt("""
                    [0] Volver
                    Seleccione una opción: 
                    """);
                switch (option)
                {
                    case 1:
                        if (producto.Stock > 0)
                            AgregarAlCarrito(producto);
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
        private static void AgregarAlCarrito(Producto producto)
        {
            while (true)
            {
                Console.Clear();
                var qty = ConsoleHelper.Inputs.GetValidInt($"""
                    AGREGAR AL CARRITO
                    ==================
                    Producto: {producto.Nombre} - $ {producto.Precio}
                    Descripción: {producto.Descripcion}
                    Stock disponible: {producto.Stock}
                    
                    Ingrese la cantidad a agregar (0 para cancelar): 
                    """);

                if (qty == 0)
                    return;

                if (qty > producto.Stock)
                {
                    ConsoleHelper.Messages.Error($"La cantidad ingresada supera el stock disponible ({producto.Stock} unidades)");
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                ConsoleHelper.Messages.OK($"Se han agregado {qty} unidad(es) al carrito de compras");
                ConsoleHelper.Messages.Continuar();
                return;
            }
        }
    }
}
