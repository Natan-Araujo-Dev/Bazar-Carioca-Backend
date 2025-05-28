using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository Repository;
        private readonly IWebService WebService;

        public ProductsController(IProductRepository repository, IWebService webService)
        {
            Repository = repository;
            WebService = webService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await Repository.GetAsync();

            return Ok(products);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Product>> GetById(int Id)
        {
            var product = await Repository.GetByIdAsync(Id);

            return Ok(product);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<ProductDTO>> Create([FromForm] ProductCreateDTO dto)
        {
            var fileUrl = "";

            if (dto.File != null)
                fileUrl = await WebService.UploadImageAsync("products", dto.File);

            // substituir por mapper
            var product = new Product
            {
                ProductTypeId = dto.ProductTypeId,
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = fileUrl,
                Stock = dto.Stock,
                Description = dto.Description
            };

            await Repository.AddAsync(product);

            // substituir por mapper
            var finalDto = new ProductDTO
            {
                ProductTypeId = dto.ProductTypeId,
                Name = dto.Name,
                Price = dto.Price,
                ImageUrl = fileUrl,
                Stock = dto.Stock,
                Description = dto.Description
            };

            return Ok(finalDto);
        }

        // HttpPost removido para ser substituido por HttpPatch

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            var product = await Repository.GetByIdAsync(Id);
            var fileUrl = product.ImageUrl;
            await WebService.DeleteFileAsync(fileUrl);

            await Repository.DeleteAsync(Id);

            return Ok($"Produto com id = {Id} apagado.");
        }
    }
}
