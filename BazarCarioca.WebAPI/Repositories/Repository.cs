using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class Repository<Entity> : IRepository<Entity>
        where Entity : class, IEntity
    {
        protected AppDbContext DataBase;

        public Repository(AppDbContext _DataBase)
        {
            DataBase = _DataBase;
        }



        public async Task<IQueryable<Entity>> GetAsync()
        {
            var entity = DataBase.Set<Entity>();

            return await Task.FromResult(entity);
        }

        public async Task<Entity> GetByIdAsync(int Id)
        {
            var entity = await DataBase.Set<Entity>()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            return entity;
        }

        public async Task AddAsync(Entity entity)
        {
            await DataBase.Set<Entity>().AddAsync(entity);
            //await CommitAsync();
        }

        public async Task<Entity> UpdateAsync(Entity entity, JsonPatchDocument<Entity> request)
        {
            request.ApplyTo(entity);

            //await CommitAsync();

            return entity;
        }

        public async Task DeleteAsync(int Id)
        {
            var entity = await DataBase.Set<Entity>().FindAsync(Id);
            DataBase.Set<Entity>().Remove(entity);

            //await CommitAsync();
        }

        public async Task CommitAsync()
        {
            await DataBase.SaveChangesAsync();
        }
    }
}
