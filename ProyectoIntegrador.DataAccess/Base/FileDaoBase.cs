namespace ProyectoIntegrador.DataAccess.Base
{
    using ProyectoIntegrador.DataAccess.Interfaces;
    public abstract class FileDaoBase<T>(string filePath)
        : IDao<T> where T : class
    {
        protected readonly string FilePath = filePath
            ?? throw new ArgumentNullException(nameof(filePath));
        protected void EnsureFileExists()
        {
            if (!File.Exists(FilePath))
            {
                var directory = Path.GetDirectoryName(FilePath);
                if (!string.IsNullOrEmpty(directory))
                    Directory.CreateDirectory(directory);
                File.Create(FilePath).Dispose();
            }
        }
        public abstract T? GetById(int id);
        public abstract IEnumerable<T> GetAll();
        public abstract int Add(T entity);
        public abstract bool Update(T entity);
        public abstract bool Delete(int id);
    }
}
