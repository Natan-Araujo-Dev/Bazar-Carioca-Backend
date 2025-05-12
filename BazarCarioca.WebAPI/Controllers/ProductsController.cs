using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        //Tudo foi comentado pois houve alteração no relacionamento entre entidades pelo FluentApi

        //[HttpGet("/Produtos/Todos")]
        //public async Task<ActionResult<List<Product>>> GetAsync()
        //{
        //    var Products = await _context.Products
        //        .AsNoTracking()
        //        .ToListAsync();

        //    if (Products.IsNullOrEmpty())
        //        return NotFound("Nenhum produto no nosso banco de dados.");

        //    return Ok(Products);
        //}

        //[HttpGet("/Produtos/{Id:int}")]
        //public async Task<ActionResult<Product>> GetByIdAsync(int Id)
        //{
        //    var Product = await _context.Products
        //        .FindAsync(Id);

        //    if (Product == null)
        //        return NotFound($"Nenhum produto com esse ID ({Id}) no nosso banco de dados.");

        //    return Ok(Product);
        //}

        //[HttpGet("/Produtos/Busca/{Busca}")]
        //public async Task<ActionResult<IEnumerable<Product>>> GetBySearchASync(string Busca)
        //{
        //    bool IsNumber = int.TryParse(Busca, out int Id);

        //    var Products = await _context.Products
        //        .AsNoTracking()
        //        .Where(p => (IsNumber && p.Id == Id)
        //                 || p.Name.Contains(Busca))
        //        .ToListAsync();

        //    if (Products.IsNullOrEmpty())
        //        return NotFound($"Nenhum produto com o termo ''{Busca}'' no nosso banco de dados.");

        //    return Ok(Products);
        //}

        //[HttpGet("/Produtos/Categoria/{Id:int}")]
        //public async Task<ActionResult<List<Product>>> GetByCategoryIdAsync(int Id)
        //{
        //    var Products = await _context.Products
        //        .AsNoTracking()
        //        .Where(p =>
        //            p.ProductTypeId == Id)
        //        .ToListAsync();

        //    if (Products.IsNullOrEmpty())
        //        return NotFound("Categoria sem produto ou categoria não encontrada.");

        //    return Ok(Products);
        //}

        //[HttpGet("/Produtos/Loja/{Id:int}")]
        //public async Task<ActionResult<List<Product>>> GetByStoreIdAsync(int Id)
        //{
        //    var Products = await _context.ProductTypes
        //        .Where(pt =>
        //            pt.StoreId == Id)
        //        .SelectMany(pt =>
        //            pt.Products)
        //        .AsNoTracking()
        //        .ToListAsync();

        //    if (Products.IsNullOrEmpty())
        //        return NotFound("Loja sem produto ou loja não encontrada.");

        //    return Ok(Products);
        //}
    }
}
