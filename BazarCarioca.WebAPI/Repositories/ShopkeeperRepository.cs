using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using static Amazon.S3.Util.S3EventNotification;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ShopkeeperRepository : Repository<Shopkeeper>, IShopkeeperRepository
    {
        public ShopkeeperRepository(AppDbContext _DataBase) : base(_DataBase)
        {

        }



        public async Task<Shopkeeper> GetByStoreIdAsync(int Id)
        {
            Shopkeeper shopkeeper = await DataBase.Shopkeepers
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            #region Log
            Console.WriteLine("===============================================");
            Console.WriteLine("\n\n");
            Console.WriteLine($"Shopkeeper = {shopkeeper}");
            Console.WriteLine("\n\n");
            Console.WriteLine("===============================================");
            #endregion

            return shopkeeper;
        }

        public async Task<Shopkeeper> GetByServiceIdAsync(int Id)
        {
            Service? service = await DataBase.Services
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            Store? store = await DataBase.Stores
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == service.StoreId);

            Shopkeeper? shopkeeper = await DataBase.Shopkeepers
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == store.ShopkeeperId);

            return shopkeeper;
        }

        public async Task<Shopkeeper> GetByProductTypeIdAsync(int Id)
        {
            ProductType? productType = await DataBase.ProductTypes
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            Store? store = await DataBase.Stores
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == productType.StoreId);

            Shopkeeper? shopkeeper = await DataBase.Shopkeepers
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == store.ShopkeeperId);

            return shopkeeper;
        }

        public async Task<Shopkeeper> GetByProductIdAsync(int Id)
        {
            Product? product = await DataBase.Products
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == Id);

            ProductType? productType = await DataBase.ProductTypes
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == product.ProductTypeId);

            Store? store = await DataBase.Stores
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == productType.StoreId);

            Shopkeeper? shopkeeper = await DataBase.Shopkeepers
                .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == store.ShopkeeperId);

            return shopkeeper;
        }
    }
}
