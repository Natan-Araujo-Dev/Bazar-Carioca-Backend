using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IProductRepository : IRepository<Product>, IImageRepository<Product, ProductUpdateDTO>
    {
        
    }
}
