using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IWebService WebService;

        public ProductRepository(AppDbContext _DataBase, IWebService _WebService) : base(_DataBase)
        {
            WebService = _WebService;
        }

        public async Task UpdateAsync(int id, Product product)
        {
            DataBase.Entry(product).State = EntityState.Modified;
            await DataBase.SaveChangesAsync();
        }
    }
}
