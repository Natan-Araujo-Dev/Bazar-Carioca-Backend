using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }
    }
}
