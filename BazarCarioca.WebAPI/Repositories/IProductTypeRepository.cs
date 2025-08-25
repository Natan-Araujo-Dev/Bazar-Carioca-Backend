using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        Task<IEnumerable<ProductType>> GetByStoreIdAsync(int Id);
        Task<IEnumerable<ProductType>> GetTreatedByStoreIdAsync(int Id);
        Task<IEnumerable<ProductType>> GetByTermAsync(string Term);
    }
}
