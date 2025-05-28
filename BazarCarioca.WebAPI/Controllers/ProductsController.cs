using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;

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

        [HttpPost]
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

        //Só funciona com Postman
        //Refinar lógica principalmente com WebService
        [HttpPatch("Atualizar/{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDTO>> Patch(int id, [FromForm] ProductPatchRequestDTO requestDto)
        {
            var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<ProductUpdateDTO>>(requestDto.PatchDocumentJson);

            var product = await Repository.GetByIdAsync(id);

            var dto = new ProductUpdateDTO
            {
                ProductTypeId = product.ProductTypeId,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                Description = product.Description
            };

            patchDoc.ApplyTo(dto, ModelState);

            if (requestDto.File != null && !requestDto.RemoveImage)
            {
                await WebService.DeleteFileAsync(product.ImageUrl);
                product.ImageUrl = await WebService.UploadImageAsync("products", requestDto.File);
            }
            else if (requestDto.RemoveImage && product.ImageUrl != "")
            {
                await WebService.DeleteFileAsync(product.ImageUrl);
                product.ImageUrl = "";
            }

            product.ProductTypeId = dto.ProductTypeId;
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.Description = dto.Description;

            await Repository.UpdateAsync(id, product);

            var result = new ProductDTO
            {
                ProductTypeId = product.ProductTypeId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                Description = product.Description
            };

            return Ok(result);
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
