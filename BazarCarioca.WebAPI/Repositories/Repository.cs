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



        public IQueryable<T> Get()
        {
            var entity = DataBase.Set<T>().AsNoTracking();

            return entity;
        }

        public T GetById(int Id)
        {
            var entity = DataBase.Set<T>().Find(Id);

            return entity;
        }

        public void Add(T entity)
        {
            DataBase.Set<T>().Add(entity);
            DataBase.SaveChanges();
        }

        public void Update(int Id, T entity)
        {
            DataBase.Entry(entity).State = EntityState.Modified;
            DataBase.Set<T>().Update(entity);
            DataBase.SaveChanges();
        }

        public void Delete(int Id)
        {
            DataBase.Set<T>().Remove(DataBase.Set<T>().Find(Id));
            DataBase.SaveChanges();
        }
    }
}
