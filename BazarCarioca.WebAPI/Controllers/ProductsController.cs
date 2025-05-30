using AutoMapper;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.DTOs.Mapper;
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
        private readonly IMapper Mapper;

        public ProductsController(IProductRepository repository, IWebService webService, IMapper mapper)
        {
            Repository = repository;
            WebService = webService;
            Mapper = mapper;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await Repository.GetAsync();
            var productsDTO = Mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ProductDTO>> GetById(int Id)
        {
            var product = await Repository.GetByIdAsync(Id);
            var productDTO = Mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Create([FromForm] ProductCreateDTO createDto)
        {
            var fileUrl = "";

            if (createDto.File != null)
                fileUrl = await WebService.UploadImageAsync("products", createDto.File);

            var product = Mapper.Map<Product>(createDto,
                opts => opts.Items["fileUrl"] = fileUrl);

            await Repository.AddAsync(product);

            var productDto = Mapper.Map<ProductDTO>(createDto,
                opts => opts.Items["fileUrl"] = fileUrl);

            return Ok(productDto);
        }

        //Só funciona com Postman por causa do JSON de PATCH
        //Refinar lógica principalmente com WebService
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDTO>> Patch(int id, [FromForm] ProductPatchRequestDTO requestDto)
        {
            var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<ProductUpdateDTO>>(requestDto.PatchDocumentJson);

            var product = await Repository.GetByIdAsync(id);

            var updateDto = Mapper.Map<ProductUpdateDTO>(product);

            patchDoc.ApplyTo(updateDto, ModelState);

            // consertar essa lógica
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

            product.ProductTypeId = updateDto.ProductTypeId;
            product.Name = updateDto.Name;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.Description = updateDto.Description;

            await Repository.UpdateAsync(id, product);

            var producDto = Mapper.Map<ProductDTO>(product);

            return Ok(producDto);
        }

        [HttpDelete("{Id:int}")]
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
