using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }
    }
}
