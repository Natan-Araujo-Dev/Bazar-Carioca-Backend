using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }

        public async Task<IEnumerable<Store>> GetByShopkeeperIdAsync(int Id)
        {
            var stores = await DataBase.stores
                 .Where(s =>  s.ShopkeeperId == Id)
                 .ToListAsync();

            if (stores is null)
                throw new ArgumentException(nameof(Store));

            return stores;
        }
    }
}
