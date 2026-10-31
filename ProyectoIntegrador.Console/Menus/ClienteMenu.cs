namespace ProyectoIntegrador.Console.Menus
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteMenu(ClienteService clienteService, ProductoService productoService)
    {
        private readonly ClienteService _clienteService = clienteService;
        private readonly ProductoService _productoService = productoService;
        internal void Show()
        {
            while (true)
            {
                var option = ConsoleHelper.Inputs.GetValidInt("""
                CLIENTE
                =======
                [1] Iniciar sesión
                [2] Registrarse
                [3] Recuperar contraseña
                [0] Volver
                Ingrese una opción: 
                """);
                switch (option)
                {
                    case 1:
                        InicioSesion();
                        break;
                    case 2:
                        Registro();
                        break;
                    case 3:
                        RecuperarContrasenia();
                        break;
                    case 0:
                        ConsoleHelper.Messages.Volviendo();
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        break;
                }
            }
        }
        private void Registro()
        {
            Console.WriteLine("""
            REGISTRO
            ========
            """);

            string? email;
            while (true)
            {
                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ");
                if (email == null)
                    return;
                if (!ClienteValidator.Email(email))
                {
                    ConsoleHelper.Messages.Error("El formato del E-mail no es válido. Intente nuevamente");
                    continue;
                }
                var resultEmail = _clienteService.ExisteEmail(email);
                if (!resultEmail.IsSuccess)
                {
                    ConsoleHelper.Messages.Error(resultEmail.Message);
                    continue;
                }
                break;
            }

            string? dni;
            while (true)
            {
                dni = ConsoleHelper.Inputs.GetValidText("DNI: ");
                if (dni == null)
                    return;
                if (!ClienteValidator.Dni(dni))
                {
                    ConsoleHelper.Messages.Error("El formato del DNI no es válido. Intente nuevamente");
                    continue;
                }
                var resultDni = _clienteService.ExisteDni(dni);
                if (!resultDni.IsSuccess)
                {
                    ConsoleHelper.Messages.Error(resultDni.Message);
                    continue;
                }
                break;
            }

            string? cuitCuil;
            while (true)
            {
                cuitCuil = ConsoleHelper.Inputs.GetValidText("CUIT / CUIL: ");
                if (cuitCuil == null)
                    return;
                if (!ClienteValidator.CuitCuil(cuitCuil))
                {
                    ConsoleHelper.Messages.Error("El formato del CUIT / CUIL no es válido. Intente nuevamente");
                    continue;
                }
                var resultCuitCuil = _clienteService.ExisteCuitCuil(cuitCuil);
                if (!resultCuitCuil.IsSuccess)
                {
                    ConsoleHelper.Messages.Error(resultCuitCuil.Message);
                    continue;
                }
                break;
            }

            string? nombre;
            while (true)
            {
                nombre = ConsoleHelper.Inputs.GetValidText("Nombre: ");
                if (nombre == null)
                    return;
                if (!ClienteValidator.Nombre(nombre))
                {
                    ConsoleHelper.Messages.Error("El formato del Nombre no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }


            string? apellido;
            while (true)
            {
                apellido = ConsoleHelper.Inputs.GetValidText("Apellido: ");
                if (apellido == null)
                    return;
                if (!ClienteValidator.Apellido(apellido))
                {
                    ConsoleHelper.Messages.Error("El formato del Apellido no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }

            string? telefono;
            while (true)
            {
                telefono = ConsoleHelper.Inputs.GetValidText("Teléfono (opcional - Enter para omitir): ", isOptional: true);
                if (telefono == null)
                    return;
                if (!ClienteValidator.Telefono(telefono))
                {
                    ConsoleHelper.Messages.Error("El formato del Teléfono no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }
            string? direccion;
            while (true)
            {
                direccion = ConsoleHelper.Inputs.GetValidText("Dirección (opcional - Enter para omitir): ", isOptional: true);
                if (direccion == null)
                    return;
                if (!ClienteValidator.Direccion(direccion))
                {
                    ConsoleHelper.Messages.Error("El formato de la Direción no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }

            string? contrasenia;
            while (true)
            {
                contrasenia = ConsoleHelper.Inputs.GetValidText("Contraseña: ");
                if (contrasenia == null)
                    return;
                if (!ClienteValidator.Contrasenia(contrasenia))
                {
                    ConsoleHelper.Messages.Error("El formato de la Contraseña no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }

            var cliente = new Cliente()
            {
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Telefono = telefono,
                Direccion = direccion,
                Email = email,
                CuitCuil = cuitCuil,
                Contrasenia = contrasenia,
                Activo = true
            };
            while (true)
            {
                var option = ConsoleHelper.Inputs.GetValidInt($"""
                DATOS CARGADOS
                ==============
                {cliente}

                ¿Desea guardar el cliente?
                [1] Sí
                [2] No
                Ingrese una opción: 
                """);
                switch (option)
                {
                    case 1:
                        ConsoleHelper.Messages.Info("Guardando datos...");
                        var result = _clienteService.RegistrarCliente(cliente);
                        if (result.IsSuccess)
                            ConsoleHelper.Messages.OK($"{result.Message} (ID : {result.Value.IdCliente})");
                        else
                            ConsoleHelper.Messages.Error(result.Message);
                        return;
                    case 2:
                        ConsoleHelper.Messages.Volviendo();
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        break;
                }
            }
        }
        private void RecuperarContrasenia()
        {
            Console.WriteLine("""
            RECUPERAR CONTRASEÑA
            ====================
            """);
            string? email;
            while (true)
            {
                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ");
                if (email == null)
                    return;
                if (!ClienteValidator.Email(email))
                {
                    ConsoleHelper.Messages.Error("El formato del E-mail no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }
            ConsoleHelper.Messages.Info("Verificando datos...");
            var result = _clienteService.RecuperarContrasenia(email);
            if (!result.IsSuccess)
            {
                ConsoleHelper.Messages.Error(result.Message);
                return;
            }
            ConsoleHelper.Messages.OK("""
            Se le ha enviado un E-Mail para poder recuperar la contraseña
            """);
        }
        private void InicioSesion()
        {
            Console.WriteLine("""
            INICIO DE SESIÓN
            ================
            """);
            string? email;
            while (true)
            {

                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ");
                if (email == null)
                    return;
                if (!ClienteValidator.Email(email))
                {
                    ConsoleHelper.Messages.Error("El formato del E-mail no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }
            string? contrasenia;
            while (true)
            {
                contrasenia = ConsoleHelper.Inputs.GetValidText("Contraseña: ");
                if (contrasenia == null)
                    return;
                if (!ClienteValidator.Contrasenia(contrasenia))
                {
                    ConsoleHelper.Messages.Error("El formato de la Contraseña no es válido. Intente nuevamente");
                    continue;
                }
                break;
            }
            ConsoleHelper.Messages.Info("Verificando datos...");
            var result = _clienteService.IniciarSesion(email, contrasenia);
            if (!result.IsSuccess)
            {
                ConsoleHelper.Messages.Error(result.Message);
                return;
            }
            var sesionMenu = new SesionMenu(result.Value!, _clienteService, _productoService);
            sesionMenu.Show();
            ConsoleHelper.Messages.Info("Sesión finalizada correctamente");
        }
        private class SesionMenu(Cliente cliente, ClienteService clienteService, ProductoService productoService)
        {
            private Cliente _cliente = cliente;
            private readonly ClienteService _clienteService = clienteService;
            private readonly ProductoService _productoService = productoService;
            internal void Show()
            {
                while (true)
                {
                    ConsoleHelper.Messages.OK($"Bienvenid@, {_cliente.Nombre} {_cliente.Apellido} [ID: {_cliente.IdCliente}]");
                    var option = ConsoleHelper.Inputs.GetValidInt("""
                    TIENDA
                    ======
                    [1] Navegar catálogo de productos
                    [2] Ver carrito de compras
                    [3] Mi perfil
                    [0] Cerrar sesión
                    Ingrese una opción: 
                    """);
                    switch (option)
                    {
                        case 1:
                            var productosMenu = new ProductosMenu(_productoService);
                            productosMenu.Show();
                            break;
                        case 2:
                            Console.WriteLine("CARRITO DE COMPRAS");
                            break;
                        case 3:
                            if (!Perfil())
                                return;
                            break;
                        case 0:
                            ConsoleHelper.Messages.Info("Cerrando sesión...");
                            return;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            break;
                    }
                }
            }
            private bool Perfil()
            {
                while (true)
                {
                    var option = ConsoleHelper.Inputs.GetValidInt("""
                    PERFIL
                    ======
                    [1] Ver datos
                    [2] Modificar datos
                    [3] Eliminar cuenta
                    [0] Volver
                    Ingrese una opción: 
                    """);
                    switch (option)
                    {
                        case 1:
                            Console.WriteLine($"""
                            DATOS
                            =====
                            {_cliente}
                            """);
                            break;
                        case 2:
                            ModificarDatos();
                            break;
                        case 3:
                            if (EliminarCuenta())
                                return false;
                            break;
                        case 0:
                            ConsoleHelper.Messages.Info("Volviendo...");
                            return true;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            break;
                    }
                }
            }
            private bool EliminarCuenta()
            {
                while (true)
                {
                    var option = ConsoleHelper.Inputs.GetValidInt($"""
                    ELIMINAR CUENTA
                    ===============
                    ¿Está seguro que desea eliminar su cuenta? Esta acción no se puede deshacer            
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);
                    switch (option)
                    {
                        case 1:
                            ConsoleHelper.Messages.Info("Eliminando cuenta...");
                            var result = _clienteService.EliminarCliente(_cliente.IdCliente);
                            if (result.IsSuccess)
                            {
                                _cliente.Activo = false;
                                ConsoleHelper.Messages.OK("Cuenta eliminada");
                                return true;
                            }
                            else
                                ConsoleHelper.Messages.Error(result.Message);
                            return false;
                        case 2:
                            ConsoleHelper.Messages.Volviendo();
                            return false;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            break;
                    }
                }
            }
            private void ModificarDatos()
            {
                var cliente = ClienteHelper.ModificarDatos(_cliente, _clienteService, false);
                while (true)
                {
                    var option = ConsoleHelper.Inputs.GetValidInt($"""
                    DATOS CARGADOS
                    ==============
                    {cliente}

                    ¿Desea guardar los cambios?
                    [1] Sí
                    [2] No
                    Ingrese una opción: 
                    """);
                    switch (option)
                    {
                        case 1:
                            ConsoleHelper.Messages.Info("Guardando cambios...");
                            var result = _clienteService.ActualizarCliente(cliente);
                            if (result.IsSuccess)
                            {
                                ConsoleHelper.Messages.OK("Datos actualizados correctamente");
                                _cliente = cliente;
                            }
                            else
                                ConsoleHelper.Messages.Error(result.Message);
                            return;
                        case 2:
                            ConsoleHelper.Messages.Volviendo();
                            return;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            break;
                    }
                }
            }
            //private void NavegarCatalogoProductos()
            //{
            //    var resultado = _productoService.ObtenerTodos();
            //    if (!resultado.IsSuccess || resultado.Value == null || !resultado.Value.Any())
            //    {
            //        ConsoleHelper.Messages.Info("No hay productos disponibles en el catálogo actualmente");
            //        return;
            //    }
            //    var productos = resultado.Value.ToList();
            //    const int pageSize = 10;
            //    int paginaActual = 1;
            //    int totalPaginas = (int)Math.Ceiling((double)productos.Count / pageSize);
            //    string input = string.Empty;
            //    do
            //    {
            //        Console.Clear();
            //        Console.ForegroundColor = ConsoleColor.Cyan;
            //        Console.WriteLine("========================================================================");
            //        Console.WriteLine($"                 CATÁLOGO DE PRODUCTOS (Página {paginaActual} de {totalPaginas})");
            //        Console.WriteLine("========================================================================");
            //        Console.ResetColor();
            //        var productosPagina = productos.Skip((paginaActual - 1) * pageSize).Take(pageSize);
            //        foreach (var prod in productosPagina)
            //        {
            //            Console.WriteLine($"[ID: {prod.IdProducto}] {prod.Nombre} - ${prod.Precio:N2} | Stock: {prod.Stock}");
            //        }
            //        Console.WriteLine("========================================================================");
            //        Console.WriteLine("");
            //        if (paginaActual < totalPaginas)
            //        {
            //            Console.WriteLine("[S] Página Siguiente");
            //        }
            //        if (paginaActual > 1)
            //        {
            //            Console.WriteLine("[A] Página Anterior");
            //        }
            //        Console.WriteLine("[0] Volver");
            //        Console.WriteLine("");
            //        Console.WriteLine("O ingrese el ID del producto que desea ver en detalle");
            //        Console.Write("Ingrese una opción: ");
            //        input = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;
            //        if (input == "0")
            //        {
            //            break;
            //        }
            //        else if (input == "S" && paginaActual < totalPaginas)
            //        {
            //            paginaActual++;
            //        }
            //        else if (input == "A" && paginaActual > 1)
            //        {
            //            paginaActual--;
            //        }
            //        else if (int.TryParse(input, out int idProducto))
            //        {
            //            var productoSeleccionado = productos.FirstOrDefault(p => p.IdProducto == idProducto);
            //            if (productoSeleccionado != null)
            //            {
            //                // TODO: En la siguiente etapa invocaremos aquí el detalle del producto seleccionado
            //                ConsoleHelper.Messages.Info($"Seleccionó el producto ID {productoSeleccionado.IdProducto}: {productoSeleccionado.Nombre}");
            //            }
            //            else
            //            {
            //                ConsoleHelper.Messages.ErrorMEssage("El ID ingresado no corresponde a un producto del catálogo");
            //            }
            //        }
            //        else
            //        {
            //            ConsoleHelper.Messages.OpcionNoValida();
            //        }

            //    } while (input != "0");
            //}
        }
    }
}
