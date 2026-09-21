namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IEmpleadoDao
        : IDao<Empleado>
    {
        Empleado? GetByEmail(string email);
        Empleado? GetByApellido(string apellido);
        Empleado? GetByLegajo(int legajo);
    }
}
