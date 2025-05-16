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
        public ActionResult<IEnumerable<ProductType>> Get()
        {
            var productTypes = Repository.Get().ToList();

            return Ok(productTypes);
        }

        [HttpGet("{Id:int}")]
        public ActionResult<ProductType> GetById(int Id)
        {
            var productTypes = Repository.GetById(Id);

            return Ok(productTypes);
        }

        [HttpPost("Criar")]
        public ActionResult<ProductType> CreateStore([FromBody] ProductType productType)
        {
            Repository.Add(productType);

            return Ok(productType);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public ActionResult<ProductType> FullUpdate(int Id, [FromBody] ProductType productType)
        {
            productType.Id = Id;
            Repository.Update(Id, productType);

            return Ok(productType);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public ActionResult<bool> DeleteStore(int Id)
        {
            var productType = Repository.GetById(Id);

            Repository.Delete(Id);

            return Ok($"Tipo de produto com id = {Id} apagado.");
        }
    }
}

