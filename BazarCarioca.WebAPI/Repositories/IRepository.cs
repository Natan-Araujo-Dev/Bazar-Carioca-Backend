namespace BazarCarioca.WebAPI.Repositories
{
    public interface IRepository<Entity>
    {
        Task<IQueryable<Entity>> GetAsync();
        Task<Entity> GetByIdAsync(int Id);
        Task AddAsync(Entity entity);
        Task UpdateAsync(int Id, Entity entity);
        Task DeleteAsync(int Id);

        Task CommitAsync();
    }
}
