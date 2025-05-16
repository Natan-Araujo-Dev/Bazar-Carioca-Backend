using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ShopkeeperRepository : Repository<Shopkeeper>, IShopkeeperRepository
    {
        public ShopkeeperRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }
    }
}
