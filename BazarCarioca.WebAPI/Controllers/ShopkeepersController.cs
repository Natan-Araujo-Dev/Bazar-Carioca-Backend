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
        public async Task<ActionResult<IEnumerable<Shopkeeper>>> Get()
        {
            var shopkeepers = await Repository.GetAsync();

            return Ok(shopkeepers);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Shopkeeper>> GetById(int Id)
        {
            var shopkeeper = await Repository.GetByIdAsync(Id);

            return Ok(shopkeeper);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<Shopkeeper>> Create(Shopkeeper shopkeeper)
        {
            await Repository.AddAsync(shopkeeper);

            return Ok(shopkeeper);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public async Task<ActionResult> Update(int Id, [FromBody] Shopkeeper shopkeeper)
        {
            await Repository.UpdateAsync(Id, shopkeeper);

            return Ok(shopkeeper);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> Delete(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Lojista com id = {Id} apagado(/a).");
        }
    }
}
