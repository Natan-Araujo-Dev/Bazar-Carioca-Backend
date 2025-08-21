using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<IEnumerable<Service>> GetByStoreIdAsync(int Id);
        Task<IEnumerable<Service>> GetByTermAsync(string Term);
    }
}
