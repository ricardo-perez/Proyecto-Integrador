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
                    return Result<Cliente>.Failure("El Email ya se encuentra registrado", TipoError.Conflicto);
                int idGenerado = _clienteDao.Add(cliente);
                if (idGenerado > 0)
                {
                    cliente.IdCliente = idGenerado;
                    return Result<Cliente>.Success(cliente, "Cliente registrado con éxito");
                }
                return Result<Cliente>.Failure("No se pudo completar el registro en la base de datos", TipoError.BaseDeDatos);
            }
            catch (DbException ex)
            {
                return Result<Cliente>.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Failure($"Ocurrió un error inesperado durante el registro: {ex.Message}", TipoError.Inesperado);
            }
        }
        public Result<Cliente?> IniciarSesion(string email, string contrasena)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                    return Result<Cliente?>.Failure("Debe ingresar el e-mail y la contraseña", TipoError.Validacion);

                var cliente = _clienteDao.GetByEmail(email.Trim());

                if (cliente is null || !cliente.Activo || cliente.Contrasenia != contrasena)
                    return Result<Cliente?>.Failure("Credenciales inválidas o cuenta inactiva", TipoError.Validacion);

                return Result<Cliente?>.Success(cliente, "Inicio de sesión exitoso");
            }
            catch (DbException ex)
            {
                return Result<Cliente?>.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente?>.Failure($"Ocurrió un error inesperado durante el inicio de sesión: {ex.Message}", TipoError.Inesperado);
            }
        }
        public Result<Cliente> ActualizarCliente(Cliente cliente)
        {
            try
            {
                Cliente? clienteExistente = null;

                clienteExistente = _clienteDao.GetById(cliente.IdCliente);
                if (clienteExistente is null)
                    return Result<Cliente>.Failure("El cliente a actualizar no existe", TipoError.NoEncontrado);

                clienteExistente = _clienteDao.GetByEmail(cliente.Email);
                if (clienteExistente != null
                    && clienteExistente.IdCliente != cliente.IdCliente)
                    return Result<Cliente>.Failure("El Email ya se encuentra registrado por otro cliente", TipoError.Conflicto);

                if (_clienteDao.Update(cliente))
                    return Result<Cliente>.Success(cliente, "Cliente actualizado con éxito");
                return Result<Cliente>.Failure("No se pudo actualizar el cliente en la base de datos", TipoError.BaseDeDatos);
            }
            catch (DbException ex)
            {
                return Result<Cliente>.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Failure($"Ocurrió un error inesperado durante la actualización: {ex.Message}", TipoError.Inesperado);
            }
        }
        public Result RecuperarContrasenia(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return Result.Failure("Debe ingresar un e-mail", TipoError.Validacion);

                var cliente = _clienteDao.GetByEmail(email.Trim());

                if (cliente is null || !cliente.Activo)
                    return Result.Failure("El e-mail no corresponde a una cuenta registrada o activa", TipoError.NoEncontrado);

                return Result.Success("Se ha enviado un correo electrónico con las instrucciones para recuperar la contraseña");
            }
            catch (DbException ex)
            {
                return Result.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Ocurrió un error inesperado al procesar la solicitud: {ex.Message}", TipoError.Inesperado);
            }
        }
    }
}
