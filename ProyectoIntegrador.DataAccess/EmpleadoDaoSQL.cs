namespace ProyectoIntegrador.DataAccess
{
    using ProyectoIntegrador.Entities;
    public class EmpleadoDaoSQL : DaoBase, IEmpleadoDao
    {
        public EmpleadoDaoSQL(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Empleado> FindAll()
        {
            throw new NotImplementedException();
        }
        public Empleado FindById(int id)
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
