using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IStoreRepository : IRepository<Store>
    {
        IEnumerable<Store> GetByShopkeeperId(int Id);
    }
}
