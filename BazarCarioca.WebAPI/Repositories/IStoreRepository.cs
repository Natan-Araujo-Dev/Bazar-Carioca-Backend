using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IStoreRepository
    {
        IEnumerable<Store> Get();

        Store GetById(int Id);

        IEnumerable<Store> GetByShopkeeperId(int Id);

        Store Create(Store store);

        Store Update(Store store);

        bool DeleteById(int Id);
    }
}
