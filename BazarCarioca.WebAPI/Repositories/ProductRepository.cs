using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
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

        public async Task<Product> AddWithImageAsync(Product product, IFormFile file)
        {
            await AddAsync(product);

            Console.WriteLine("Id é: " + product.Id);

            var fileName = product.Id.ToString();
            var fileUrl = await WebService.UploadImageAsync("products", fileName, file);

            product.ImageUrl = fileUrl;
            await CommitAsync();

            return product;
        }
    }
}
