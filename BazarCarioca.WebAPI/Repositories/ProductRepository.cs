using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }
        public async Task PatchAsync(int Id, JsonPatchDocument<Product> patchDoc)
        {
            var product = await DataBase.Products.FindAsync(Id);

            patchDoc.ApplyTo(product);

            DataBase.Entry(product).State = EntityState.Modified;
            
            await DataBase.SaveChangesAsync();
        }
    }
}
