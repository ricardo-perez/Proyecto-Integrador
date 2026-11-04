namespace ProyectoIntegrador.Console.Menus.Checkout
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class CheckoutMainMenu(Cliente cliente, List<ItemCarrito> carrito, MedioDePagoService medioDePagoService, ProductoService productoService)
    {
        private readonly Cliente _cliente = cliente;
        private readonly List<ItemCarrito> _carrito = carrito;
        private readonly MedioDePagoService _medioDePagoService = medioDePagoService;
        private readonly ProductoService _productoService = productoService;
        public void Show(decimal total)
        {
            if (!CheckStock())
                return;

            var result = _medioDePagoService.ObtenerMediosDePago();
            if (!result.IsSuccess || result.Value == null || result.Value.Count == 0)
            {
                ConsoleHelper.Messages.Error("Ocurrió un error en el sistema");
                ConsoleHelper.Messages.Error("No es posible cargar los medios de pago");
                ConsoleHelper.Messages.Error("Vuelva a intentar más tarde");
                ConsoleHelper.Messages.Continuar();
                return;
            }

            var mediosDePago = result.Value;

            Console.Clear();
            Console.WriteLine("""
            FINALIZAR COMPRA
            ================
            """);
            var indice = 0;
            foreach (var medio in mediosDePago)
            {
                Console.WriteLine($"[{++indice}] {medio.Nombre.ToUpper()} - {medio.Descripcion}");
            }
            Console.Write("""
                [0] Cancelar
                Seleccione el medio de pago: 
                """);

            var input = Console.ReadLine()?.Trim();
            if (!int.TryParse(input, out var opcionPago)
                || opcionPago < 0
                || opcionPago > mediosDePago.Count)
            {
                ConsoleHelper.Messages.OpcionNoValida();
                ConsoleHelper.Messages.Continuar();
                return;
            }
            if (opcionPago == 0)
                return;

            var medioDePagoSeleccionado = mediosDePago[opcionPago - 1];
            int idMedioDePago = medioDePagoSeleccionado.IdMedioDePago;

            ConsoleHelper.Messages.Info("Procesando compra de forma segura...");

            bool checkoutOk = true;

            if (checkoutOk)
            {
                Console.Clear();
                ConsoleHelper.Messages.OK("Compra realizada con éxito");
                Console.WriteLine($"Medio de pago: {medioDePagoSeleccionado.Nombre}");
                Console.WriteLine($"Total abonado: $ {total}");
                _carrito.Clear();
            }
            else
            {
                ConsoleHelper.Messages.Error("Ocurrió un error en el sistema");
                ConsoleHelper.Messages.Error("No se pudo procesar la venta");
                ConsoleHelper.Messages.Error("Vuelva a intentar más tarde");
            }
            ConsoleHelper.Messages.Continuar();
        }
        private bool CheckStock()
        {
            foreach (var item in _carrito)
            {
                var stock = _productoService.ObtenerStock(item.Producto.IdProducto);
                if (item.Cantidad > stock)
                {
                    Console.Clear();
                    ConsoleHelper.Messages.Error($"El producto '{item.Producto.Nombre}' ya no cuenta con stock suficiente.");
                    ConsoleHelper.Messages.Error($"Stock disponible actual: {stock} unidad(es).");
                    ConsoleHelper.Messages.Continuar();
                    return false;
                }
            }
            return true;
        }
    }
}
