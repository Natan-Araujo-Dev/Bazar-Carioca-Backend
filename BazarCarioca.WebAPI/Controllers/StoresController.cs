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
        public ActionResult<IEnumerable<Store>> Get()
        {
            var stores = Repository.Get().ToList();

            return Ok(stores);
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Store> GetById(int Id)
        {
            var store = Repository.GetById(Id);

            return Ok(store);
        }

        [HttpGet("Lojista/{Id:int}")]
        public ActionResult<IEnumerable<Store>> GetStoresByShopkeeperId(int Id)
        {
            var stores = Repository.GetByShopkeeperId(Id);

            return Ok(stores);
        }

        [HttpPost("Criar")]
        public ActionResult<Store> CreateStore([FromBody] Store store)
        {
            Repository.Add(store);

            return Ok(store);
        }

        // Com erro. Devo investigar
        [HttpPut("Atualizar/{Id:int}")]
        public ActionResult<Store> FullUpdate(int Id, [FromBody] Store store)
        {
            store.Id = Id;
            Repository.Update(Id, store);

            return Ok(store);
        }

        // Implementar isso no Repository
        //[HttpPatch("Atualizar/{id:int}")]
        //public ActionResult<Store> PatchStore(int id, [FromBody] JsonPatchDocument<Store> patchDoc)
        //{
        //    if (patchDoc == null)
        //        return BadRequest();

        //    var store = Repository.PartialUpdate(id, patchDoc);

        //    return store;
        //}

        [HttpDelete("Apagar/{Id:int}")]
        public ActionResult<bool> DeleteStore(int Id)
        {
            var store = Repository.GetById(Id);

            Repository.Delete(Id);

            return Ok($"Loja com id = {Id} apagada.");
        }
    }
}
