using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Serviços")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository Repository;

        public ServicesController(IServiceRepository repository)
        {
            Repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Store>> Get()
        {
            var services = Repository.Get().ToList();

            return Ok(services);
        }

        [HttpGet("{Id:int}")]
        public ActionResult<Service> GetById(int Id)
        {
            var service = Repository.GetById(Id);

            return Ok(service);
        }

        [HttpPost("Criar")]
        public ActionResult<Service> CreateStore([FromBody] Service service)
        {
            Repository.Add(service);

            return Ok(service);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public ActionResult<Store> FullUpdate(int Id, [FromBody] Service service)
        {
            service.Id = Id;
            Repository.Update(Id, service);

            return Ok(service);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public ActionResult<bool> DeleteStore(int Id)
        {
            var service = Repository.GetById(Id);

            Repository.Delete(Id);

            return Ok($"Serviço com id = {Id} apagado.");
        }
    }
}