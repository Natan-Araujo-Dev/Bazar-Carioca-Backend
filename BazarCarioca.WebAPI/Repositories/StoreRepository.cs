using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class StoreRepository : ImageRepository<Store, StoreUpdateDTO>, IStoreRepository
    {
        public StoreRepository(AppDbContext _DataBase, IWebService _WebService, IMapper _Mapper) : base(_DataBase, _WebService, _Mapper)
        {
        }

        public async Task<IEnumerable<Store>> GetByShopkeeperIdAsync(int Id)
        {
            var stores = await DataBase.Stores
                 .Where(s =>  s.ShopkeeperId == Id)
                 .ToListAsync();

            if (stores is null)
                throw new ArgumentException(nameof(Store));

            return stores;
        }

        public async Task<IEnumerable<Store>> GetByTermAsync(string Term)
        {
            var stores = await DataBase.Stores
                    .Where(x => x.Name.ToLower().Contains(Term))
                    .ToListAsync();

            return stores;
        }
    }
}
