using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository Repository;

        public StoresController(IStoreRepository repository)
        {
            Repository = repository;
        }

        [HttpGet("Lojas")]
        public ActionResult<IEnumerable<Store>> GetStores()
        {
            var stores = Repository.Get();

            return Ok(stores);
        }

        [HttpGet("Lojas/{Id:int}")]
        public ActionResult<Store> GetStoreById(int Id)
        {
            var store = Repository.GetById(Id);

            return Ok(store);
        }

        [HttpGet("Lojas/Lojista/{Id:int}")]
        public ActionResult<IEnumerable<Store>> GetStoresByShopkeeperId(int Id)
        {
            var stores = Repository.GetByShopkeeperId(Id);

            return Ok(stores);
        }

        [HttpPost("Lojas/Criar")]
        public ActionResult<Store> CreateStore([FromBody] Store Store)
        {
            var store = Repository.Create(Store);

            return Ok(store);
        }

        [HttpPut("Lojas/Atualizar/{Id:int}")]
        public ActionResult<Store> UpdateStore(int Id, [FromBody] Store Store)
        {
            Store.Id = Id;
            var updated = Repository.Update(Store);
            return Ok(updated);
        }

        [HttpDelete("Lojas/Apagar/{Id:int}")]
        public ActionResult<bool> DeleteStore(int Id)
        {
            var Store = Repository.DeleteById(Id);

            return Ok($"Loja com id = {Id} apagada.");
        }
    }
}
