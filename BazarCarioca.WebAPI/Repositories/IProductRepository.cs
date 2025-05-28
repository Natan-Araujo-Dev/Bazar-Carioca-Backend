using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using System.Threading.Tasks;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(int id, Product product);
    }
}
