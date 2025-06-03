using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IStoreRepository : IRepository<Store>, IImageRepository<Store>
    {
        Task<IEnumerable<Store>> GetByShopkeeperIdAsync(int Id);
    }
}
