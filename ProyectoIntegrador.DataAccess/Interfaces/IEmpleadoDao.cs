namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using ProyectoIntegrador.Entities;
    public interface IEmpleadoDao
        : IDao<Empleado>
    {
        Empleado? GetByEmail(string email);
        IEnumerable<Empleado> SearchByEmail(string email);
        Empleado? GetByApellido(string apellido);
        IEnumerable<Empleado> SearchByApellido(string apellido);
        Empleado? GetByLegajo(int legajo);
    }
}
