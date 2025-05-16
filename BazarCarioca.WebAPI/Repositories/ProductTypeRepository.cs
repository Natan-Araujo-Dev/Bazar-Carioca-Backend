using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }
    }
}
