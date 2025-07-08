using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("Bazar-Carioca/Produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IWebService WebService;
        private readonly IMapper Mapper;

        public ProductsController(IUnitOfWork _UnitOfWork, IWebService webService, IMapper mapper)
        {
            UnitOfWork = _UnitOfWork;
            WebService = webService;
            Mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await UnitOfWork.ProductRepository.GetAsync();

            if (products.IsNullOrEmpty())
                return NotFound("Nenhum produto foi encotrado.");

            var productsDTO = Mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(Id);

            if (product == null)
                return NotFound($"O produto com Id = {Id} não foi encontrado.");

            var productDTO = Mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var product = Mapper.Map<Product>(createDto);

            if (createDto.File != null)
            {
                product = await UnitOfWork.ProductRepository.AddWithImageAsync(product, createDto.File);
                await UnitOfWork.CommitAsync();
            }
            else
            {
                await UnitOfWork.ProductRepository.AddAsync(product);
                await UnitOfWork.CommitAsync();
            }

            var productDto = Mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        //Só funciona com Postman por causa do JSON de PATCH
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await UnitOfWork.ProductRepository.UpdateWithImageAsync(Id, requestDto);
            await UnitOfWork.CommitAsync();

            var productDto = Mapper.Map<ProductDTO>(productPatched);

            return Ok(productDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(Id);

            if (product == null)
                return NotFound($"O produto não foi apagado pois não existe um produto com Id = {Id}.");

            var fileUrl = product.ImageUrl;

            if (fileUrl != null)
                await WebService.DeleteFileAsync(fileUrl);

            await UnitOfWork.ProductRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"O produto com id = {Id} apagado.");
        }

        /// <summary>
        /// Método somente para desenvolvimento. NÂO implemente.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var products = await UnitOfWork.ProductRepository.GetAsync();

            if (products.IsNullOrEmpty())
                return NotFound("Nenhum produto foi apagado pois não existem produtos cadastrados.");

            var ids = new List<int>();
            var imageUrls = new List<string>();

            foreach (var product in products)
            {
                ids.Add(product.Id);
                imageUrls.Add(product.ImageUrl);
            }

            for (int i = 0; i < ids.Count; i++)
            {
                if (imageUrls[i] != null)
                    await WebService.DeleteFileAsync(imageUrls[i]);

                await UnitOfWork.ProductRepository.DeleteAsync(ids[i]);
                await UnitOfWork.CommitAsync();
            }

            return Ok("Todos produtos e suas imagens foram apagadas.");
        }
    }
}