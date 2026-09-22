namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    public class ClienteService(IClienteDao clienteDao)
    {
        private readonly IClienteDao _clienteDao = clienteDao;
        public Result<Cliente> RegistrarCliente(Cliente cliente)
        {
            try
            {
                if (_clienteDao.GetByEmail(cliente.Email) != null)
                    return Result<Cliente>.Failure("El Email ya se encuentra registrado.", TipoError.Conflicto);
                int idGenerado = _clienteDao.Add(cliente);
                if (idGenerado > 0)
                {
                    cliente.IdCliente = idGenerado;
                    return Result<Cliente>.Success(cliente, "Cliente registrado con éxito.");
                }
                return Result<Cliente>.Failure("No se pudo completar el registro en la base de datos.", TipoError.BaseDeDatos);
            }
            catch (System.Data.Common.DbException ex)
            {
                return Result<Cliente>.Failure($"Error de comunicación con la base de datos: {ex.Message}", TipoError.BaseDeDatos);
            }
            catch (Exception ex)
            {
                return Result<Cliente>.Failure($"Ocurrió un error inesperado durante el registro: {ex.Message}", TipoError.Inesperado);
            }
        }
        public Cliente? IniciarSesion(string email, string contrasena)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contrasena))
                return null;
            var cliente = _clienteDao.GetByEmail(email.Trim());
            if (cliente is null || !cliente.Activo)
                return null;
            if (cliente.Contrasena != contrasena)
                return null;
            return cliente;
        }
    }
}
