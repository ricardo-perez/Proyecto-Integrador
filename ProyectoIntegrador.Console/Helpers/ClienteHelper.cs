namespace ProyectoIntegrador.Console.Helpers
{
    using ProyectoIntegrador.Entities;
    using System;
    internal static class ClienteHelper
    {
        internal static Cliente ModificarDatos(Cliente cliente, bool esEmpleado = false)
        {
            Console.WriteLine("""
                MODIFICAR DATOS
                ===============
                """);

            var infoText = "Ingrese un nuevo valor o presione ENTER para mantener el valor actual";

            var nombreInput = ConsoleHelper.Inputs.GetValidText($"""
                Nombre  [{cliente.Nombre}]
                {infoText}: 
                """, 1, 100, isOptional: true);
            var nombre = string.IsNullOrWhiteSpace(nombreInput)
                ? cliente.Nombre
                : nombreInput;

            var apellidoInput = ConsoleHelper.Inputs.GetValidText($"""
                Apellido  [{cliente.Apellido}]
                {infoText}: 
                """, 1, 100, isOptional: true);
            var apellido = string.IsNullOrWhiteSpace(apellidoInput)
                ? cliente.Apellido
                : apellidoInput;

            var dniInput = ConsoleHelper.Inputs.GetValidText($"""
                DNI [{cliente.Dni}]
                {infoText}: 
                """, 1, 15, isOptional: true);
            var dni = string.IsNullOrWhiteSpace(dniInput)
                ? cliente.Dni
                : dniInput;

            var telefonoInput = ConsoleHelper.Inputs.GetValidText($"""
                Teléfono [{cliente.Telefono ?? "N/A"}]
                {infoText}: 
                """, 1, 30, isOptional: true);
            var telefono = string.IsNullOrWhiteSpace(telefonoInput)
                ? cliente.Telefono
                : telefonoInput;

            var direccionInput = ConsoleHelper.Inputs.GetValidText($"""
                Dirección [{cliente.Direccion ?? "N/A"}]
                {infoText}: 
                """, 1, 200, isOptional: true);
            var direccion = string.IsNullOrWhiteSpace(direccionInput)
                ? cliente.Direccion
                : direccionInput;

            var emailInput = ConsoleHelper.Inputs.GetValidText($"""
                E-Mail [{cliente.Email}]
                {infoText}: 
                """, 1, 254, isOptional: true);
            var email = string.IsNullOrWhiteSpace(emailInput)
                ? cliente.Email
                : emailInput;

            var cuitCuilInput = ConsoleHelper.Inputs.GetValidText($"""
                CUIT / CUIL [{cliente.CuitCuil}]
                {infoText}: 
                """, 1, 13, isOptional: true);
            var cuitCuil = string.IsNullOrWhiteSpace(cuitCuilInput)
                ? cliente.CuitCuil
                : cuitCuilInput;

            var contrasenaInput = ConsoleHelper.Inputs.GetValidText("""
                NUEVA CONTRASEÑA
                (dejar en blanco para mantener la actual): 
                """, 1, 255, isOptional: true);
            var contrasena = string.IsNullOrWhiteSpace(contrasenaInput)
                ? cliente.Contrasenia
                : contrasenaInput;

            var activo = cliente.Activo;
            if (esEmpleado)
            {
                var estadoTexto = cliente.Activo ? "S" : "N";
                var activoInput = ConsoleHelper.Inputs.GetValidText($"¿Cuenta activa? (S/N) [{estadoTexto}]: ", 1, 1, isOptional: true);

                if (!string.IsNullOrWhiteSpace(activoInput))
                    activo = activoInput.Equals("S", StringComparison.OrdinalIgnoreCase);
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
                Contrasenia = contrasena,
                Activo = activo
            };
        }
    }
}