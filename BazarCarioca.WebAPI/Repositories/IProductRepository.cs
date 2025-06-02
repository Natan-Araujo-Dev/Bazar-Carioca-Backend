using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> AddWithImageAsync(Product product, IFormFile file);
    }
}
