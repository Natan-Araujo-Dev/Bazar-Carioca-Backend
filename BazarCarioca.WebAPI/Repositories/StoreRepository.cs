using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
namespace BazarCarioca.WebAPI.Repositories
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }

        public IEnumerable<Store> GetByShopkeeperId(int Id)
        {
            var stores = DataBase.stores
                 .Where(s =>  s.ShopkeeperId == Id)
                 .ToList();

            if (stores is null)
                throw new ArgumentException(nameof(Store));

            return stores;
        }

        //public Store PartialUpdate(int id, JsonPatchDocument<Store> patchDoc)
        //{
        //    if (patchDoc == null)
        //        throw new ArgumentNullException("Store vazia recebida.");

        //    var store = DataBase.stores
        //        .Find(id);
        //    if (store == null)
        //        throw new KeyNotFoundException($"Loja com Id = {id} não encontrada.");

        //    patchDoc.ApplyTo(store);

        //    DataBase.SaveChanges();

        //    return store;
        //}
    }
}
