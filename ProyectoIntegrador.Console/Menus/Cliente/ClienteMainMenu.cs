namespace ProyectoIntegrador.Console.Menus.Cliente
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Console.Helpers;
    using ProyectoIntegrador.Entities;
    using System;
    internal class ClienteMainMenu(ClienteService clienteService, ProductoService productoService)
    {
        private readonly ClienteService _clienteService = clienteService;
        private readonly ProductoService _productoService = productoService;
        internal void Show()
        {
            while (true)
            {
                Console.Clear();
                Console.Write("""
                CLIENTE
                =======
                [1] Iniciar sesión
                [2] Registrarse
                [3] Recuperar contraseña
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
                        Login();
                        break;
                    case 2:
                        Signup();
                        break;
                    case 3:
                        RecoveryPassword();
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
        private void Signup()
        {
            var cancelKeyword = "0";
            Console.Clear();
            Console.WriteLine("""
            REGISTRO (Ingrese 0 en cualquier campo para cancelar)
            =====================================================
            """);

            string? email;
            while (true)
            {
                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ", cancelKeyword: cancelKeyword);
                if (email == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                dni = ConsoleHelper.Inputs.GetValidText("DNI: ", cancelKeyword: cancelKeyword);
                if (dni == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                cuitCuil = ConsoleHelper.Inputs.GetValidText("CUIT / CUIL: ", cancelKeyword: cancelKeyword);
                if (cuitCuil == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                nombre = ConsoleHelper.Inputs.GetValidText("Nombre: ", cancelKeyword: cancelKeyword);
                if (nombre == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                apellido = ConsoleHelper.Inputs.GetValidText("Apellido: ", cancelKeyword: cancelKeyword);
                if (apellido == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                contrasenia = ConsoleHelper.Inputs.GetValidText("Contraseña: ", cancelKeyword: cancelKeyword);
                if (contrasenia == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                Console.Clear();
                Console.Write($"""
                DATOS CARGADOS
                ==============
                {cliente}

                ¿Desea guardar el cliente?
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
                        ConsoleHelper.Messages.Info("Guardando datos...");
                        var result = _clienteService.RegistrarCliente(cliente);
                        if (result.IsSuccess)
                            ConsoleHelper.Messages.OK($"{result.Message} (ID : {result.Value.IdCliente})");
                        else
                            ConsoleHelper.Messages.Error(result.Message);
                        ConsoleHelper.Messages.Continuar();
                        return;
                    case 2:
                        ConsoleHelper.Messages.Volviendo();
                        ConsoleHelper.Messages.Continuar();
                        return;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private void RecoveryPassword()
        {
            var cancelKeyword = "0";
            Console.Clear();
            Console.WriteLine("""
            RECUPERAR CONTRASEÑA  (Ingrese 0 en cualquier campo para cancelar)
            ==================================================================
            """);
            string? email;
            while (true)
            {
                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ", cancelKeyword: cancelKeyword);
                if (email == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                ConsoleHelper.Messages.Continuar();
                return;
            }
            ConsoleHelper.Messages.OK("""
            Se le ha enviado un E-Mail para poder recuperar la contraseña
            """);
            ConsoleHelper.Messages.Continuar();
        }
        private void Login()
        {
            var cancelKeyword = "0";
            Console.Clear();
            Console.WriteLine("""
            INICIO DE SESIÓN  (Ingrese 0 en cualquier campo para cancelar)
            ==============================================================
            """);
            string? email;
            while (true)
            {

                email = ConsoleHelper.Inputs.GetValidText("E-Mail: ", cancelKeyword: cancelKeyword);
                if (email == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
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
                contrasenia = ConsoleHelper.Inputs.GetValidText("Contraseña: ", cancelKeyword: cancelKeyword);
                if (contrasenia == null)
                {
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    return;
                }
                break;
            }
            ConsoleHelper.Messages.Info("Verificando datos...");
            var result = _clienteService.IniciarSesion(email, contrasenia);
            if (!result.IsSuccess)
            {
                ConsoleHelper.Messages.Error(result.Message);
                ConsoleHelper.Messages.Continuar();
                return;
            }
            var clienteSesionMenu = new ClienteSesionMenu(result.Value!, _clienteService, _productoService);
            clienteSesionMenu.Show();
            ConsoleHelper.Messages.Info("Sesión finalizada correctamente");
            ConsoleHelper.Messages.Continuar();
        }
    }
}
