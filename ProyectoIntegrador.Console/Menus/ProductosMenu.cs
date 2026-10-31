namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
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
                Console.WriteLine("""
                PRODUCTOS
                =========
                """);
                var totalPaginas = _productoService.ObtenerTotalPaginas(pageSize);
                var result = _productoService.ObtenerPaginaConStock(pageNumber, pageSize);
                if (!result.IsSuccess || result.Value == null || result.Value.Count == 0)
                {
                    ConsoleHelper.Messages.Info("No hay productos disponibles en el catálogo");
                    return;
                }
                var productos = result.Value;
                var indice = 0;
                foreach (var producto in productos)
                {
                    Console.WriteLine($"""
                    [{++indice}] {producto.Nombre} - $ {producto.Precio} | {producto.Descripcion} | STOCK: {producto.Stock}
                    """);
                }

                var input = ConsoleHelper.Inputs.GetValidText("""
                [P] Primer página
                [A] Anterior
                [S] Siguiente
                [U] Última página
                [0] Volver
                Seleccione una opción: 
                """);

                if (string.IsNullOrWhiteSpace(input))
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    continue;
                }

                if (int.TryParse(input.Trim(), out var option))
                {
                    if (option == 0)
                    {
                        ConsoleHelper.Messages.Info("Volviendo...");
                        return;
                    }
                    if (option > 0 && option <= productos.Count)
                    {
                        var producto = productos[option - 1];
                        Console.WriteLine($"{producto.DescripcionLarga}");
                        // Método Mostrar Descripción con opciones de compra
                    }
                    else
                    {
                        ConsoleHelper.Messages.OpcionNoValida();
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
                            pageNumber = totalPaginas > 0
                                ? totalPaginas
                                : 1;
                            break;
                        case "S":
                            if (pageNumber < totalPaginas)
                                pageNumber++;
                            else
                                ConsoleHelper.Messages.Error("Ya se encuentra en la última página");
                            break;
                        case "A":
                            if (pageNumber > 1)
                                pageNumber--;
                            else
                                ConsoleHelper.Messages.Error("Ya se encuentra en la primer página");
                            break;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            break;
                    }
                }
            }
        }
    }
}
