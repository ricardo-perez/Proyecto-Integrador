namespace ProyectoIntegrador.Business
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    using System;
    using System.Data.Common;

    public class EmpleadoService(IEmpleadoDao empleadoDao)
    {
        private readonly IEmpleadoDao _empleadoDao = empleadoDao;
        public Result<Empleado?> IniciarSesion(string nombreUsuario, string contrasenia)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasenia))
                    return Result<Empleado?>.Failure("Debe ingresar el usuario y la contraseña", ErrorType.Validation);

                var empleado = _empleadoDao.GetByNombreUsuario(nombreUsuario.Trim());

                if (empleado == null || !empleado.Activo || empleado.Contrasenia != contrasenia)
                    return Result<Empleado?>.Failure("Credenciales inválidas o cuenta inactiva", ErrorType.Validation);

                return Result<Empleado?>.Success(empleado, "Inicio de sesión exitoso");
            }
            catch (DbException ex)
            {
                return Result<Empleado?>.Failure($"Error de comunicación con la base de datos: {ex.Message}", ErrorType.Database);
            }
            catch (Exception ex)
            {
                return Result<Empleado?>.Failure($"Ocurrió un error inesperado durante el inicio de sesión: {ex.Message}", ErrorType.Unexpected);
            }
        }
    }
}
