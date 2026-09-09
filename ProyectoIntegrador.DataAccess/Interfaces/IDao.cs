namespace ProyectoIntegrador.DataAccess.Interfaces
{
    using System.Collections.Generic;
    public interface IDao<T> where T : class
    {
        int Insert(T entity);
        T? FindById(int id);
        IEnumerable<T> FindAll();
        bool Update(T entity);
        void Delete(int id);
    }
}