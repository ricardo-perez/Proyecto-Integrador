namespace ProyectoIntegrador.AccesoDatos
{
    using System.Collections.Generic;
    public interface IDao<T> where T : class
    {
        IEnumerable<T> ObtenerTodos();
        T? ObtenerPorId(int id);
        int Insertar(T entidad);
        bool Actualizar(T entidad);
        bool Eliminar(int id);
    }
}
