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

            string? nombreInput = ConsoleHelper.Inputs.GetValidText($"""
                Nombre  [{cliente.Nombre}]
                {infoText}: 
                """, 1, 100, isOptional: true);
            string nombre = string.IsNullOrWhiteSpace(nombreInput)
                ? cliente.Nombre
                : nombreInput;

            string? apellidoInput = ConsoleHelper.Inputs.GetValidText($"""
                Apellido  [{cliente.Apellido}]
                {infoText}: 
                """, 1, 100, isOptional: true);
            string apellido = string.IsNullOrWhiteSpace(apellidoInput)
                ? cliente.Apellido
                : apellidoInput;

            string? dniInput = ConsoleHelper.Inputs.GetValidText($"""
                DNI [{cliente.Dni}]
                {infoText}: 
                """, 1, 15, isOptional: true);
            string dni = string.IsNullOrWhiteSpace(dniInput)
                ? cliente.Dni
                : dniInput;

            string? telefonoInput = ConsoleHelper.Inputs.GetValidText($"""
                Teléfono [{cliente.Telefono ?? "N/A"}]
                {infoText}: 
                """, 1, 30, isOptional: true);
            string? telefono = string.IsNullOrWhiteSpace(telefonoInput)
                ? cliente.Telefono
                : telefonoInput;

            string? direccionInput = ConsoleHelper.Inputs.GetValidText($"""
                Dirección [{cliente.Direccion ?? "N/A"}]
                {infoText}: 
                """, 1, 200, isOptional: true);
            string? direccion = string.IsNullOrWhiteSpace(direccionInput)
                ? cliente.Direccion
                : direccionInput;

            string? emailInput = ConsoleHelper.Inputs.GetValidText($"""
                E-Mail [{cliente.Email}]
                {infoText}: 
                """, 1, 254, isOptional: true);
            string email = string.IsNullOrWhiteSpace(emailInput)
                ? cliente.Email
                : emailInput;

            string? cuitCuilInput = ConsoleHelper.Inputs.GetValidText($"""
                CUIT / CUIL [{cliente.CuitCuil}]
                {infoText}: 
                """, 1, 13, isOptional: true);
            string cuitCuil = string.IsNullOrWhiteSpace(cuitCuilInput)
                ? cliente.CuitCuil
                : cuitCuilInput;

            string? contrasenaInput = ConsoleHelper.Inputs.GetValidText("""
                NUEVA CONTRASEÑA
                (dejar en blanco para mantener la actual): 
                """, 1, 255, isOptional: true);
            string contrasena = string.IsNullOrWhiteSpace(contrasenaInput)
                ? cliente.Contrasenia
                : contrasenaInput;

            bool activo = cliente.Activo;
            if (esEmpleado)
            {
                string estadoTexto = cliente.Activo ? "S" : "N";
                string? activoInput = ConsoleHelper.Inputs.GetValidText($"¿Cuenta activa? (S/N) [{estadoTexto}]: ", 1, 1, isOptional: true);

                if (!string.IsNullOrWhiteSpace(activoInput))
                {
                    activo = activoInput.Equals("S", StringComparison.OrdinalIgnoreCase);
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
                Contrasenia = contrasena,
                Activo = activo
            };
        }
    }
}