namespace ProyectoIntegrador.Console.Helpers
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Entities;
    using System;
    internal static class ClienteHelper
    {
        internal static Cliente ModificarDatos(Cliente cliente, ClienteService clienteService, bool esEmpleado = false)
        {
            var clienteUpdated = new Cliente()
            {
                IdCliente = cliente.IdCliente,
                Nombre = cliente.Nombre,
                Apellido = cliente.Apellido,
                Dni = cliente.Dni,
                Telefono = cliente.Telefono,
                Direccion = cliente.Direccion,
                Email = cliente.Email,
                CuitCuil = cliente.CuitCuil,
                Contrasenia = cliente.Contrasenia,
                Activo = cliente.Activo
            };

            var infoText = "Ingrese un nuevo valor o presione ENTER para mantener el valor actual";

            while (true)
            {
                Console.Clear();
                Console.Write($"""
                MODIFICAR DATOS
                ===============
                [1] Nombre:      [{clienteUpdated.Nombre}]
                [2] Apellido:    [{clienteUpdated.Apellido}]
                [3] DNI:         [{clienteUpdated.Dni}]
                [4] Teléfono:    [{clienteUpdated.Telefono ?? "N/A"}]
                [5] Dirección:   [{clienteUpdated.Direccion ?? "N/A"}]
                [6] E-Mail:      [{clienteUpdated.Email}]
                [7] CUIT / CUIL: [{clienteUpdated.CuitCuil}]
                [8] Contraseña:  [********]
                {(esEmpleado
                    ? $"[9] Estado:      [{(clienteUpdated.Activo ? "Activo" : "Inactivo")}]"
                    : "")}
                [10] Guardar
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

                if (option == 0)
                {
                    if (Confirm("Si vuelve se perderán los cambios. Está seguro?"))
                    {
                        return cliente;
                    }
                    continue;
                }
                if (option == 10)
                {
                    if (Confirm("Se guardarán los cambios. Está seguro?"))
                    {
                        return clienteUpdated;
                    }
                    continue;
                }

                Console.WriteLine($"""
                
                {infoText}
                """);
                switch (option)
                {
                    case 1:
                        {
                            Console.Write($"Nombre [{clienteUpdated.Nombre}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Nombre(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del Nombre no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                clienteUpdated.Nombre = value;
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.Write($"Apellido [{clienteUpdated.Apellido}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Apellido(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del Apellido no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                clienteUpdated.Apellido = value;
                            }
                            break;
                        }
                    case 3:
                        {
                            Console.Write($"DNI [{clienteUpdated.Dni}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Dni(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del DNI no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                if (value != clienteUpdated.Dni)
                                {
                                    var result = clienteService.ExisteDni(value);
                                    if (!result.IsSuccess)
                                    {
                                        ConsoleHelper.Messages.Error(result.Message);
                                        ConsoleHelper.Messages.Continuar();
                                        break;
                                    }
                                }
                                clienteUpdated.Dni = value;
                            }
                            break;
                        }
                    case 4:
                        {
                            Console.Write($"Teléfono [{clienteUpdated.Telefono}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Telefono(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del Teléfono no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                clienteUpdated.Telefono = value;
                            }
                            break;
                        }
                    case 5:
                        {
                            Console.Write($"Dirección [{clienteUpdated.Direccion}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Direccion(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato de la Dirección no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                clienteUpdated.Direccion = value;
                            }
                            break;
                        }
                    case 6:
                        {
                            Console.Write($"E-Mail [{clienteUpdated.Email}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Email(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del E-Mail no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                if (value != clienteUpdated.Email)
                                {
                                    var result = clienteService.ExisteEmail(value);
                                    if (!result.IsSuccess)
                                    {
                                        ConsoleHelper.Messages.Error(result.Message);
                                        ConsoleHelper.Messages.Continuar();
                                        break;
                                    }
                                }
                                clienteUpdated.Email = value;
                            }
                            break;
                        }
                    case 7:
                        {
                            Console.Write($"CUIT / CUIL [{clienteUpdated.CuitCuil}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.CuitCuil(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato del CUIT/CUIL no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                if (value != clienteUpdated.CuitCuil)
                                {
                                    var result = clienteService.ExisteCuitCuil(value);
                                    if (!result.IsSuccess)
                                    {
                                        ConsoleHelper.Messages.Error(result.Message);
                                        ConsoleHelper.Messages.Continuar();
                                        break;
                                    }
                                }
                                clienteUpdated.CuitCuil = value;
                            }
                            break;
                        }
                    case 8:
                        {
                            Console.Write($"Contraseña [{clienteUpdated.Contrasenia}]:  ");
                            var value = Console.ReadLine()?.Trim();
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (!ClienteValidator.Contrasenia(value))
                                {
                                    ConsoleHelper.Messages.Error("El formato de la Contraseña no es válido");
                                    ConsoleHelper.Messages.Continuar();
                                    break;
                                }
                                clienteUpdated.Contrasenia = value;
                            }
                            break;
                        }
                    case 9:
                        {
                            UpdateActive(clienteUpdated);
                            break;
                        }
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        break;
                }
            }
        }
        private static bool Confirm(string mensaje)
        {
            while (true)
            {
                Console.Clear();
                Console.Write($"""
                    {mensaje}
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
                        return true;
                    case 2:
                        return false;
                    default:
                        ConsoleHelper.Messages.OpcionNoValida();
                        ConsoleHelper.Messages.Continuar();
                        continue;
                }
            }
        }
        private static void UpdateActive(Cliente cliente)
        {
            Console.Write($"""
                ¿Cuenta activa? [{(cliente.Activo ? "Sí" : "No")}]
                [1] Sí
                [2] No
                Ingrese una opción: 
                """);

            var input = Console.ReadLine()?.Trim();
            var option = int.TryParse(input, out var parsed) ? parsed : 0;

            switch (option)
            {
                case 1:
                    cliente.Activo = true;
                    break;
                case 2:
                    cliente.Activo = false;
                    break;
                default:
                    Console.WriteLine("Cancelando...");
                    ConsoleHelper.Messages.Continuar();
                    break;
            }
        }
    }
}