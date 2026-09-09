namespace ProyectoIntegrador.DataAccess.Implementations.SqlServer
{
    using ProyectoIntegrador.DataAccess.Base;
    using ProyectoIntegrador.DataAccess.Interfaces;
    using ProyectoIntegrador.Entities;
    public class EmpleadoDaoSQL(IConnectionFactory connectionFactory) : DaoBase(connectionFactory), IEmpleadoDao
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Empleado> FindAll()
        {
            throw new NotImplementedException();
        }
        public Empleado? FindById(int id)
        {
            throw new NotImplementedException();
        }
        public void Insert(Empleado entity)
        {
            throw new NotImplementedException();
        }
        public void Update(Empleado entity)
        {
            throw new NotImplementedException();
        }
    }
}
