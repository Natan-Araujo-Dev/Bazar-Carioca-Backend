using Microsoft.AspNetCore.JsonPatch;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IRepository<Entity>
        where Entity : class
    {
        Task<IQueryable<Entity>> GetAsync();
        Task<Entity> GetByIdAsync(int Id);
        Task AddAsync(Entity entity);
        Task<Entity> UpdateAsync(Entity entity, JsonPatchDocument<Entity> request);
        Task DeleteAsync(int Id);

        Task CommitAsync();
    }
}
