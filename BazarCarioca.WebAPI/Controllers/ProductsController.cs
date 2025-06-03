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
            var product = Mapper.Map<Product>(createDto);

            if (createDto.File != null)
                product = await Repository.AddWithImageAsync(product, createDto.File);
            else
                Console.WriteLine("Não era pra ter vindo pra cá...");
                //await Repository.AddAsync(product);

            var productDto = Mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        //Só funciona com Postman por causa do JSON de PATCH
        //Refinar lógica principalmente com WebService
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductDTO>> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            var productPatched = await Repository.UpdateWithImageAsync(Id, requestDto);

            var productDto = Mapper.Map<ProductDTO>(productPatched);

            return Ok(productDto);
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
