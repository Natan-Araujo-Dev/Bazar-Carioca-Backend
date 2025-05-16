namespace BazarCarioca.WebAPI.Repositories
{
    public interface IRepository<T>
    {
        IQueryable<T> Get();
        T GetById(int Id);
        void Add(T entity);
        void Update(int Id, T entity);
        void Delete(int Id);
    }
}
