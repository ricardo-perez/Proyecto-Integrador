namespace ProyectoIntegrador.Console.Menus.Compras
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ComprasMainMenu(Cliente cliente, VentaService ventaService)
    {
        private readonly Cliente _cliente = cliente;
        private readonly VentaService _ventaService = ventaService;
        public void Show()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("""
                HISTORIAL DE COMPRAS
                ====================
                """);

                var result = _ventaService.HistorialPorCliente(_cliente.IdCliente);

                if (!result.IsSuccess
                    || result.Value == null
                    || result.Value.Count == 0)
                {
                    Console.WriteLine("No realizó compras");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }

                var compras = result.Value;
                int indice = 0;
                foreach (var venta in compras)
                    Console.WriteLine($"[{++indice}] Compra N° {venta.IdVenta} - Fecha: {venta.Fecha:dd/MM/yyyy HH:mm} - Total: $ {venta.MontoTotal:N2}");

                Console.Write("""
                    [0] Volver
                    Ingrese una opción: 
                    """);

                var input = Console.ReadLine()?.Trim();
                if (!int.TryParse(input, out var opcion)
                    || opcion < 0
                    || opcion > compras.Count)
                {
                    ConsoleHelper.Messages.OpcionNoValida();
                    ConsoleHelper.Messages.Continuar();
                    continue;
                }

                if (opcion == 0)
                    return;

                var compra = compras[opcion - 1];

                ShowDetail(compra);
            }
        }
        private static void ShowDetail(Venta compra)
        {
            Console.Clear();
            Console.WriteLine($"""
            DETALLE DE COMPRA N° {compra.IdVenta}
            =====================================
            Fecha: {compra.Fecha:dd/MM/yyyy HH:mm}
            -------------------------------------
            Subtotal: $ {compra.Subtotal:N2}
            Descuento: $ {compra.DescuentoTotal:N2}
            Total: $ {compra.MontoTotal:N2}
            """);
            ConsoleHelper.Messages.Continuar();
        }
    }
}
