namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System.Data.Common;
    public class ClienteService(IClienteDao clienteDao)
    {
        private readonly IClienteDao _clienteDao = clienteDao;
        public Result<Cliente> RegistrarCliente(Cliente cliente)
        {
            try
            {
                if (_clienteDao.GetByEmail(cliente.Email) != null)
                    return Result<Cliente>.Failure("El Email ya se encuentra registrado", ErrorType.Conflicto);
                var idCliente = _clienteDao.Add(cliente);
                if (idCliente > 0)
                {
                    cliente.IdCliente = idCliente;
                    return Result<Cliente>.Success(cliente, "Cliente registrado con éxito");
                }
                return Result<Cliente>.Failure("No se pudo completar el registro en la base de datos", ErrorType.BaseDeDatos);
            }
            catch (DbException ex)
            {
                return Result<Cliente>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Failure($"Ocurrió un error inesperado durante el registro: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result<Cliente?> IniciarSesion(string email, string contrasena)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                    return Result<Cliente?>.Failure("Debe ingresar el E-Mail y la contraseña", ErrorType.Validacion);

                var cliente = _clienteDao.GetByEmail(email.Trim());

                if (cliente == null || !cliente.Activo || cliente.Contrasenia != contrasena)
                    return Result<Cliente?>.Failure("Credenciales inválidas o cuenta inactiva", ErrorType.Validacion);

                return Result<Cliente?>.Success(cliente, "Inicio de sesión exitoso");
            }
            catch (DbException ex)
            {
                return Result<Cliente?>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente?>.Failure($"Ocurrió un error inesperado durante el inicio de sesión: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result<Cliente> ActualizarCliente(Cliente cliente)
        {
            try
            {
                Cliente? clienteExistente = null;

                clienteExistente = _clienteDao.GetById(cliente.IdCliente);
                if (clienteExistente == null)
                    return Result<Cliente>.Failure("El cliente a actualizar no existe", ErrorType.NoEncontrado);

                clienteExistente = _clienteDao.GetByEmail(cliente.Email);
                if (clienteExistente != null
                    && clienteExistente.IdCliente != cliente.IdCliente)
                    return Result<Cliente>.Failure("El Email ya se encuentra registrado por otro cliente", ErrorType.Conflicto);

                if (_clienteDao.Update(cliente))
                    return Result<Cliente>.Success(cliente, "Cliente actualizado con éxito");
                return Result<Cliente>.Failure("No se pudo actualizar el cliente en la base de datos", ErrorType.BaseDeDatos);
            }
            catch (DbException ex)
            {
                return Result<Cliente>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Failure($"Ocurrió un error inesperado durante la actualización: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result RecuperarContrasenia(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result.Failure("Debe ingresar un E-Mail", ErrorType.Validacion);

                var cliente = _clienteDao.GetByEmail(email.Trim());

                if (cliente == null || !cliente.Activo)
                    return Result.Failure("El E-Mail no corresponde a una cuenta registrada o activa", ErrorType.NoEncontrado);

                return Result.Success("Se ha enviado un correo electrónico con las instrucciones para recuperar la contraseña");
            }
            catch (DbException ex)
            {
                return Result.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Ocurrió un error inesperado al procesar la solicitud: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result EliminarCliente(int idCliente)
        {
            if (idCliente <= 0)
                return Result.Failure("Usuario no válido", ErrorType.Validacion);
            try
            {
                var deleted = _clienteDao.Delete(idCliente);
                if (deleted)
                    return Result.Success("Cliente eliminado con éxito");
                return Result.Failure("No se pudo encontrar el cliente o ya se encuentra inactivo", ErrorType.BaseDeDatos);
            }
            catch (DbException ex)
            {
                return Result.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Ocurrió un error inesperado durante la eliminación: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result<bool> ExisteEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result<bool>.Failure("El E-Mail ingresado no es válido", ErrorType.Validacion);
                var emailExists = (_clienteDao.GetByEmail(email.Trim()) != null);
                var message = emailExists
                    ? "El E-Mail ya se encuentra registrado en el sistema"
                    : "El E-Mail se encuentra disponible";
                return Result<bool>.Success(emailExists, message);
            }
            catch (DbException ex)
            {
                return Result<bool>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Ocurrió un error inesperado al verificar el E-Mail: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result<bool> ExisteDni(string dni)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dni))
                    return Result<bool>.Failure("El DNI ingresado no es válido", ErrorType.Validacion);

                var dniExists = (_clienteDao.GetByDni(dni.Trim()) != null);
                var message = dniExists
                    ? "El DNI ya se encuentra registrado"
                    : "El DNI se encuentra está disponible";
                return Result<bool>.Success(dniExists, message);
            }
            catch (DbException ex)
            {
                return Result<bool>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Ocurrió un error inesperado al verificar el DNI: {ex.Message}", ErrorType.Inesperado);
            }
        }
        public Result<bool> ExisteCuitCuil(string cuitCuil)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cuitCuil))
                    return Result<bool>.Failure("El CUIT/CUIL ingresado no es válido", ErrorType.Validacion);
                var cuitCuilExists = _clienteDao.GetByCuitCuil(cuitCuil.Trim()) != null;
                var message = cuitCuilExists
                    ? "El CUIT/CUIL ya se encuentra registrado"
                    : "El CUIT/CUIL se ecuentra disponible";
                return Result<bool>.Success(cuitCuilExists, message);
            }
            catch (DbException ex)
            {
                return Result<bool>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Ocurrió un error inesperado al verificar el CUIT/CUIL: {ex.Message}", ErrorType.Inesperado);
            }
        }
    }
}
