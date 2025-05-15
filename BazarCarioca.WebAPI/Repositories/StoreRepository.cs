using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext DataBase;
        public StoreRepository(AppDbContext _DataBase)
        {
            DataBase = _DataBase;
        }

        public IEnumerable<Store> Get()
        {
            var stores = DataBase.stores
                .ToList();

            if (stores is null)
                throw new ArgumentException(nameof(stores));

            return stores;
        }

        public Store GetById(int Id)
        {
            var store = DataBase.stores
                 .Find(Id);

            if (store is null)
                throw new ArgumentException(nameof(store));

            return store;
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

        public Store Create(Store store)
        {
            if (store is null)
                throw new ArgumentNullException(nameof(store));

            DataBase.stores.Add(store);
            DataBase.SaveChanges();

            return store;
        }

        public Store Update(Store store)
        {
            if (store == null)
                throw new ArgumentNullException("Nenhuma loja foi passada na header.");

            var newStore = DataBase.stores.Find(store.Id);

            if (newStore == null)
                throw new ArgumentException($"Loja com Id = {store.Id} não encontrada.");

            newStore.Name = store.Name;
            newStore.Description = store.Description;
            newStore.ImageUrl = store.ImageUrl;
            newStore.CellphoneNumber = store.CellphoneNumber;
            newStore.Neighborhood = store.Neighborhood;
            newStore.Street = store.Street;
            newStore.Number = store.Number;
            newStore.OpeningTime = store.OpeningTime;
            newStore.ClosingTime = store.ClosingTime;

            DataBase.SaveChanges();
            return newStore;
        }

        public bool DeleteById(int Id)
        {
            var store = DataBase.stores
                 .Find(Id);

            if (store is null)
                throw new ArgumentException(nameof(Store));

            DataBase.stores.Remove(store);
            DataBase.SaveChanges();

            return true;
        }
    }
}
