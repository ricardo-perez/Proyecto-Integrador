namespace ProyectoIntegrador.Console.Helpers
{
    using ProyectoIntegrador.Business;
    using ProyectoIntegrador.Entities;
    using System;
    internal static class ClienteHelper
    {
        internal static Cliente ModificarDatos(Cliente cliente, ClienteService clienteService, bool esEmpleado = false)
        {
            Console.WriteLine("""
                MODIFICAR DATOS
                ===============
                """);

            var infoText = "Ingrese un nuevo valor o presione ENTER para mantener el valor actual";

            string? nombre;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    Nombre  [{cliente.Nombre}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    nombre = cliente.Nombre;
                    break;
                }
                if (!ClienteValidator.Nombre(input))
                {
                    ConsoleHelper.Messages.Error("El formato del Nombre no es válido. Intente nuevamente");
                    continue;
                }
                nombre = input;
                break;
            }

            string? apellido;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    Apellido  [{cliente.Apellido}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    apellido = cliente.Apellido;
                    break;
                }
                if (!ClienteValidator.Apellido(input))
                {
                    ConsoleHelper.Messages.Error("El formato del Apellido no es válido. Intente nuevamente");
                    continue;
                }
                apellido = input;
                break;
            }

            string? dni;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    DNI [{cliente.Dni}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    dni = cliente.Dni;
                    break;
                }
                if (!ClienteValidator.Dni(input))
                {
                    ConsoleHelper.Messages.Error("El formato del DNI no es válido. Intente nuevamente");
                    continue;
                }
                if (input != cliente.Dni)
                {
                    var reslut = clienteService.ExisteDni(input);
                    if (!reslut.IsSuccess)
                    {
                        ConsoleHelper.Messages.Error(reslut.Message);
                        continue;
                    }
                }
                dni = input;
                break;
            }

            string? telefono;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    Teléfono [{cliente.Telefono ?? "N/A"}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    telefono = cliente.Telefono;
                    break;
                }
                if (!ClienteValidator.Telefono(input))
                {
                    ConsoleHelper.Messages.Error("El formato del Teléfono no es válido. Intente nuevamente");
                    continue;
                }
                telefono = input;
                break;
            }

            string? direccion;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    Dirección [{cliente.Direccion ?? "N/A"}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    direccion = cliente.Direccion;
                    break;
                }
                if (!ClienteValidator.Direccion(input))
                {
                    ConsoleHelper.Messages.Error("El formato de la Dirección no es válido. Intente nuevamente");
                    continue;
                }
                direccion = input;
                break;
            }

            string? email;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    E-Mail [{cliente.Email}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    email = cliente.Email;
                    break;
                }
                if (!ClienteValidator.Email(input))
                {
                    ConsoleHelper.Messages.Error("El formato del E-mail no es válido. Intente nuevamente");
                    continue;
                }
                if (input != cliente.Email)
                {
                    var result = clienteService.ExisteEmail(input);
                    if (!result.IsSuccess)
                    {
                        ConsoleHelper.Messages.Error(result.Message);
                        continue;
                    }
                }
                email = input;
                break;
            }

            string? cuitCuil;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText($"""
                    CUIT / CUIL [{cliente.CuitCuil}]
                    {infoText}: 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    cuitCuil = cliente.CuitCuil;
                    break;
                }
                if (!ClienteValidator.CuitCuil(input))
                {
                    ConsoleHelper.Messages.Error("El formato del CUIT / CUIL no es válido. Intente nuevamente");
                    continue;
                }
                if (input != cliente.CuitCuil)
                {
                    var result = clienteService.ExisteCuitCuil(input);
                    if (!result.IsSuccess)
                    {
                        ConsoleHelper.Messages.Error(result.Message);
                        continue;
                    }
                }
                cuitCuil = input;
                break;
            }

            string? contrasenia;
            while (true)
            {
                var input = ConsoleHelper.Inputs.GetValidText("""
                    NUEVA CONTRASEÑA
                    (dejar en blanco para mantener la actual): 
                    """, isOptional: true);
                if (string.IsNullOrWhiteSpace(input))
                {
                    contrasenia = cliente.Contrasenia;
                    break;
                }
                if (!ClienteValidator.Contrasenia(input))
                {
                    ConsoleHelper.Messages.Error("El formato de la Contraseña no es válido. Intente nuevamente");
                    continue;
                }
                contrasenia = input;
                break;
            }

            var activo = cliente.Activo;
            if (esEmpleado)
            {
                while (true)
                {
                    var activoTexto = cliente.Activo ? "Sí" : "No";
                    var option = ConsoleHelper.Inputs.GetValidInt($"""
                        ¿Cuenta activa? [{activoTexto}]
                        [1] Sí
                        [2] No
                        Ingrese una opción: 
                        """);
                    switch (option)
                    {
                        case 1:
                            activo = true;
                            break;
                        case 2:
                            activo = false;
                            break;
                        default:
                            ConsoleHelper.Messages.OpcionNoValida();
                            continue;
                    }
                    break;
                }
            }

            return new Cliente
            {
                IdCliente = cliente.IdCliente,
                Nombre = nombre,
                Apellido = apellido,
                Dni = dni,
                Telefono = telefono,
                Direccion = direccion,
                Email = email,
                CuitCuil = cuitCuil,
                Contrasenia = contrasenia,
                Activo = activo
            };
        }
    }
}