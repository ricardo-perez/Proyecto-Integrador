namespace ProyectoIntegrador.AccesoDatos
{
    using System.Collections.Generic;
    public interface IDao<T> where T : class
    {
        void Insert(T entity);
        T FindById(int id);
        IEnumerable<T> FindAll();
        void Update(T entity);
        void Delete(int id);
    }
}