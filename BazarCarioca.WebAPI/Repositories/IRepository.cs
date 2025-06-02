namespace BazarCarioca.WebAPI.Repositories
{
    public interface IRepository<T>
    {
        Task<IQueryable<T>> GetAsync();
        Task<T> GetByIdAsync(int Id);
        Task AddAsync(T entity);
        Task UpdateAsync(int Id, T entity);
        Task DeleteAsync(int Id);

        Task CommitAsync();
    }
}
