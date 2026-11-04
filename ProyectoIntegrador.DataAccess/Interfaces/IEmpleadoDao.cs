namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IEmpleadoDao
        : IDao<Empleado>
    {
        Empleado? GetByNombreUsuario(string nombreUsuario);
    }
}
