using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductRepository : ImageRepository<Product, ProductUpdateDTO>, IProductRepository
    {
        public ProductRepository(AppDbContext _DataBase, IWebService _WebService, IMapper _Mapper) :
            base(_DataBase, _WebService, _Mapper)
        {
        }

        public async Task<IEnumerable<Product>> GetByProductTypeIdAsync(int Id)
        {
            var products = await DataBase.Products
                 .Where(s => s.ProductTypeId == Id)
                 .ToListAsync();

            if (products is null)
                throw new ArgumentException(nameof(Product));

            return products;
        }

        public async Task<IEnumerable<Product>> GetByTermAsync(string Term)
        {
            var products = await DataBase.Products
                    .Where(x => x.Name.ToLower().Contains(Term))
                    .ToListAsync();

            return products;
        }
    }
}
