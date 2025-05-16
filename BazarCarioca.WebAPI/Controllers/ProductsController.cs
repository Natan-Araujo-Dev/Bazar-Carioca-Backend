using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository Repository;

        public ProductsController(IProductRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = Repository.Get().ToList();

            return Ok(products);
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Product> GetById(int Id)
        {
            var products = Repository.GetById(Id);

            return Ok(products);
        }

        [HttpPost("Criar")]
        public ActionResult<Product> CreateStore([FromBody] Product product)
        {
            Repository.Add(product);

            return Ok(product);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public ActionResult<Product> FullUpdate(int Id, [FromBody] Product product)
        {
            product.Id = Id;
            Repository.Update(Id, product);

            return Ok(product);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public ActionResult<bool> DeleteStore(int Id)
        {
            var product = Repository.GetById(Id);

            Repository.Delete(Id);

            return Ok($"Produto com id = {Id} apagado.");
        }
    }
}
