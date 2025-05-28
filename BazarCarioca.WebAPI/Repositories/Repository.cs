using BazarCarioca.WebAPI.Context;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext DataBase;

        public Repository(AppDbContext _DataBase)
        {
            DataBase = _DataBase;
        }



        public async Task<IQueryable<T>> GetAsync()
        {
            var entity = DataBase.Set<T>().AsNoTracking();

            return await Task.FromResult(entity);
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            var entity = await DataBase.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            return entity;
        }

        public async Task AddAsync(T entity)
        {
            await DataBase.Set<T>().AddAsync(entity);
            await Commit();
        }

        public async Task UpdateAsync(int Id, T entity)
        {
            DataBase.Entry(entity).State = EntityState.Modified;
            DataBase.Set<T>().Update(entity);
            await Commit();
        }

        public async Task DeleteAsync(int Id)
        {
            var entity = await DataBase.Set<T>().FindAsync(Id);
            DataBase.Set<T>().Remove(entity);
            await Commit();
        }



        private async Task Commit()
        {
            await DataBase.SaveChangesAsync();
        }
    }
}
