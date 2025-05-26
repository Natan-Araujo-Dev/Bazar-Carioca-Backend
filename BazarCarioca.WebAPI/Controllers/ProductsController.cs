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
        public async Task<ActionResult<Product>> Create([FromForm] CreateProductDTO DTO)
        {
            var fileUrl = "";

            if (DTO.File != null)
                fileUrl = await WebService.UploadFileAsync(DTO.File);

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

            if (DTO.RemoveImage || DTO.File == null)
            {
                fileUrl = "";
            }
            else
            {
                fileUrl = await WebService.UploadFileAsync(DTO.File);
                fileUrl = fileUrl.ToString();
            }

            var product = new Product
            {
                Id = Id,
                ProductTypeId = DTO.ProductTypeId,
                Name = DTO.Name,
                Price = DTO.Price,
                ImageUrl = fileUrl,
                Stock = DTO.Stock,
                Description = DTO.Description
            };

            await Repository.UpdateAsync(Id, product);

            return Ok(product);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Produto com id = {Id} apagado.");
        }
    }
}
