using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Lojistas")]
    [ApiController]
    public class ShopkeepersController : ControllerBase
    {
        private readonly IShopkeeperRepository Repository;
        public ShopkeepersController(IShopkeeperRepository _Repository) 
        {
            Repository = _Repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Shopkeeper>> Get()
        {
            var shopkeepers = Repository.Get().ToList();

            return Ok(shopkeepers);
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Shopkeeper> GetById(int Id)
        {
            var shopkeeper = Repository.GetById(Id);

            return Ok(shopkeeper);
        }

        [HttpPost("Criar")]
        public ActionResult<Shopkeeper> Create(Shopkeeper shopkeeper)
        {
            Repository.Add(shopkeeper);

            return Ok(shopkeeper);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public ActionResult Update(int Id, [FromBody] Shopkeeper shopkeeper)
        {
            Repository.Update(Id, shopkeeper);

            return Ok(shopkeeper);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public ActionResult<bool> Delete(int Id)
        {
            var shopkeeper = Repository.GetById(Id);

            Repository.Delete(Id);

            return Ok($"Lojista com id = {Id} apagado(/a).");
        }
    }
}
