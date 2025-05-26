using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Lojas")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository Repository;

        public StoresController(IStoreRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> Get()
        {
            var stores = await Repository.GetAsync();

            return Ok(stores);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Store>> GetById(int Id)
        {
            var store = await Repository.GetByIdAsync(Id);

            return Ok(store);
        }

        [HttpGet("Lojista/{Id:int}")]
        public async Task<ActionResult<IEnumerable<Store>>> GetStoresByShopkeeperId(int Id)
        {
            var stores = await Repository.GetByShopkeeperIdAsync(Id);

            return Ok(stores);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<Store>> CreateStore([FromBody] Store store)
        {
            await Repository.AddAsync(store);

            return Ok(store);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public async Task<ActionResult<Store>> FullUpdate(int Id, [FromBody] Store store)
        {
            store.Id = Id;
            await Repository.UpdateAsync(Id, store);

            return Ok(store);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Loja com id = {Id} apagada.");
        }
    }
}
