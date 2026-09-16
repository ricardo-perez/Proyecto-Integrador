namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using System.Collections.Generic;
    public interface IDao<T> where T : class
    {
        int Add(T entity);
        T? GetById(int id);
        IEnumerable<T> GetAll();
        bool Update(T entity);
        bool Delete(int id);
    }
}