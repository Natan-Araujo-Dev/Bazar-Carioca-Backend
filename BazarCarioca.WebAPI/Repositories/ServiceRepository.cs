using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext _DataBase) : base(_DataBase)
        {

        }

        public async Task<IEnumerable<Service>> GetByStoreIdAsync(int Id)
        {
            var services = await DataBase.Services
                 .Where(s => s.StoreId == Id)
                 .ToListAsync();

            if (services is null)
                throw new ArgumentException(nameof(Service));

            return services;
        }

        public async Task<IEnumerable<Service>> GetByTermAsync(string Term)
        {
            var services = await DataBase.Services
                    .Where(x => x.Name.ToLower().Contains(Term))
                    .ToListAsync();

            return services;
        }
    }
}
