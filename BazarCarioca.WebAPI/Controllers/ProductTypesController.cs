using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Tipos_de_Produtos")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepository Repository;

        public ProductTypeController(IProductTypeRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productTypes = await Repository.GetAsync();

            return Ok(productTypes);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<ProductType>> GetById(int Id)
        {
            var productTypes = await Repository.GetByIdAsync(Id);

            return Ok(productTypes);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<ProductType>> CreateStore([FromBody] ProductType productType)
        {
            await Repository.AddAsync(productType);

            return Ok(productType);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public async Task<ActionResult<ProductType>> FullUpdate(int Id, [FromBody] ProductType productType)
        {
            productType.Id = Id;
            await Repository.UpdateAsync(Id, productType);

            return Ok(productType);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Tipo de produto com id = {Id} apagado.");
        }
    }
}

