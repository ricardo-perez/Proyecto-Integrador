namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClientePerfilMenu(Cliente cliente, ClienteService clienteService)
    {
        private Cliente _cliente = cliente;
        private readonly ClienteService _clienteService = clienteService;
        internal bool Show()
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