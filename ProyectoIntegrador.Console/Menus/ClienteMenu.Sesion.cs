namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal partial class ClienteMenu
    {
        private class SesionMenu(Cliente cliente, ClienteService clienteService, ProductoService productoService)
        {
            private Cliente _cliente = cliente;
            private readonly ClienteService _clienteService = clienteService;
            private readonly ProductoService _productoService = productoService;
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
                            var productosMenu = new ProductosMenu(_productoService);
                            productosMenu.Show();
                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("""
                                CARRITO DE COMPRAS
                                ==================
                                """);
                            ConsoleHelper.Messages.Continuar();
                            break;
                        case 4:
                            if (!Profile())
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
            private bool Profile()
            {
                while (true)
                {
                    Console.Clear();
                    Console.Write("""
                    PERFIL
                    ======
                    [1] Ver datos
                    [2] Modificar datos
                    [3] Eliminar cuenta
                    [0] Volver
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
                            Console.Clear();
                            Console.WriteLine($"""
                            DATOS
                            =====
                            {_cliente}
                            """);
                            ConsoleHelper.Messages.Continuar();
                            break;
                        case 2:
                            UpdateAccount();
                            break;
                        case 3:
                            if (DeleteAccount())
                                return false;
                            break;
                        case 0:
                            return true;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            ConsoleHelper.Messages.Continuar();
                            break;
                    }
                }
            }
            private bool DeleteAccount()
            {
                while (true)
                {
                    Console.Clear();
                    Console.Write($"""
                    ELIMINAR CUENTA
                    ===============
                    ¿Está seguro que desea eliminar su cuenta? Esta acción no se puede deshacer            
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
                            ConsoleHelper.Messages.Info("Eliminando cuenta...");
                            var result = _clienteService.EliminarCliente(_cliente.IdCliente);
                            if (result.IsSuccess)
                            {
                                _cliente.Activo = false;
                                ConsoleHelper.Messages.OK("Cuenta eliminada");
                                ConsoleHelper.Messages.Continuar();
                                return true;
                            }
                            else
                            {
                                ConsoleHelper.Messages.Error(result.Message);
                                ConsoleHelper.Messages.Continuar();
                            }
                            return false;
                        case 2:
                            return false;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            ConsoleHelper.Messages.Continuar();
                            break;
                    }
                }
            }
            private void UpdateAccount()
            {
                var cliente = ClienteHelper.ModificarDatos(_cliente, _clienteService, false);

                if (cliente == _cliente)
                    return;

                ConsoleHelper.Messages.Info("Guardando cambios...");
                var result = _clienteService.ActualizarCliente(cliente);
                if (result.IsSuccess)
                {
                    ConsoleHelper.Messages.OK("Datos actualizados correctamente");
                    _cliente = cliente;
                }
                else
                    ConsoleHelper.Messages.Error(result.Message);
                ConsoleHelper.Messages.Continuar();
            }
        }
    }
}
