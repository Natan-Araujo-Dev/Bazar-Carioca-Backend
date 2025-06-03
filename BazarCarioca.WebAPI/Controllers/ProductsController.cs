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
        public async Task<IActionResult> Get()
        {
            var products = await Repository.GetAsync();

            if (products == Empty)
                return NotFound("Nenhum produto no banco de dados");

            var productsDTO = Mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await Repository.GetByIdAsync(Id);

            if (product == null)
                return NotFound("O produto com esse Id não foi encontrado");

            var productDTO = Mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var product = Mapper.Map<Product>(createDto);

            if (createDto.File != null)
                product = await Repository.AddWithImageAsync(product, createDto.File);
            else
                await Repository.AddAsync(product);

            var productDto = Mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        //Só funciona com Postman por causa do JSON de PATCH
        //Refinar lógica principalmente com WebService
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await Repository.UpdateWithImageAsync(Id, requestDto);

            var productDto = Mapper.Map<ProductDTO>(productPatched);

            return Ok(productDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteStore(int Id)
        {
            var product = await Repository.GetByIdAsync(Id);

            if (product == null)
                return NotFound($"Produto não foi apagado pois não existe um produto com Id = {Id}");

            var fileUrl = product.ImageUrl;

            await WebService.DeleteFileAsync(fileUrl);

            await Repository.DeleteAsync(Id);

            return Ok($"Produto com id = {Id} apagado.");
        }
    }
}