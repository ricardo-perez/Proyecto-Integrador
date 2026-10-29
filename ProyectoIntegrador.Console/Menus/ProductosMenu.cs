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
            var productosPorPagina = 10;
            var paginaActual = 1;
            while (true)
            {
                Console.WriteLine("""
                PRODUCTOS
                =========
                """);
                var totalPaginas = _productoService.ObtenerTotalPaginas(productosPorPagina);
                var resultado = _productoService.ObtenerPaginaConStock(paginaActual, productosPorPagina);
                if (!resultado.IsSuccess || resultado.Value == null || resultado.Value.Count == 0)
                {
                    ConsoleHelper.Messages.Info("No hay productos disponibles en el catálogo actualmente");
                    return;
                }
                var productos = resultado.Value;
                var indice = 0;
                foreach (var producto in productos)
                {
                    Console.WriteLine($"""
                    [{++indice}] {producto.Nombre} - $ {producto.Precio} | {producto.Descripcion} | STOCK: {producto.Stock}
                    """);
                }
                Console.WriteLine("""
                [P] Primer página
                [A] Anterior
                [S] Siguiente
                [U] Última página
                [0] Volver
                """);
                var opcionSeleccionada = ConsoleHelper.Inputs.GetValidInt("""
                Seleccione una opción: 
                """);


                if (opcionSeleccionada == 0)
                {
                    ConsoleHelper.Messages.Info("Volviendo...");
                    return;
                }
                if (opcionSeleccionada > 0 && opcionSeleccionada <= productos.Count)
                {
                    var productoSeleccionado = productos[opcionSeleccionada - 1];
                    Console.WriteLine($"{productoSeleccionado.DescripcionLarga}");
                    //VerDetalleProducto(productoSeleccionado);
                }
                else
                    ConsoleHelper.Messages.OpcionNoValida();
            }

        }
    }
}
