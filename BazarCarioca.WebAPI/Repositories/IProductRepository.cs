using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task PatchAsync(int Id, JsonPatchDocument<Product> patchDoc);
    }
}
