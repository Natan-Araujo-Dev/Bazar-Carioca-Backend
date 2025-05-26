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
            var products = await Repository.GetByIdAsync(Id);

            return Ok(products);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<Product>> Create([FromForm] ProductCreateDTO DTO)
        {
            var fileUrl = "";
            if (DTO.File != null)
                fileUrl = await WebService.UploadImageAsync("products", DTO.File);

            var product = new Product
            {
                ProductTypeId = DTO.ProductTypeId,
                Name = DTO.Name,
                Price = DTO.Price,
                ImageUrl = fileUrl.ToString(),
                Stock = DTO.Stock,
                Description = DTO.Description
            };

            await Repository.AddAsync(product);

            return Ok(product);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public async Task<ActionResult<Product>> FullUpdate(int Id, [FromForm] ProductDTO DTO)
        {
            var fileUrl = "";

            var amazonProduct = await Repository.GetByIdAsync(Id);

            if (DTO.RemoveImage)
            {
                await WebService.DeleteFileAsync(amazonProduct.ImageUrl);

                fileUrl = "";
            }
            else
            {
                await WebService.DeleteFileAsync(amazonProduct.ImageUrl);

                fileUrl = await WebService.UploadImageAsync("products", DTO.File);
                fileUrl = fileUrl.ToString();
            }

            var newProduct = new Product
            {
                Id = Id,
                ProductTypeId = DTO.ProductTypeId,
                Name = DTO.Name,
                Price = DTO.Price,
                ImageUrl = fileUrl,
                Stock = DTO.Stock,
                Description = DTO.Description
            };

            await Repository.UpdateAsync(Id, newProduct);

            return Ok(newProduct);
        }

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
