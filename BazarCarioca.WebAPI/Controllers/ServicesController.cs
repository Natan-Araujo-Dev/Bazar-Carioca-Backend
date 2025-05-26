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
        public async Task<ActionResult<IEnumerable<Store>>> Get()
        {
            var services = await Repository.GetAsync();

            return Ok(services);
        }

        [HttpGet("{Id:int}")]
        public async Task<ActionResult<Service>> GetById(int Id)
        {
            var service = await Repository.GetByIdAsync(Id);

            return Ok(service);
        }

        [HttpPost("Criar")]
        public async Task<ActionResult<Service>> CreateStore([FromBody] Service service)
        {
            await Repository.AddAsync(service);

            return Ok(service);
        }

        [HttpPut("Atualizar/{Id:int}")]
        public async Task<ActionResult<Store>> FullUpdate(int Id, [FromBody] Service service)
        {
            service.Id = Id;
            await Repository.UpdateAsync(Id, service);

            return Ok(service);
        }

        [HttpDelete("Apagar/{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Serviço com id = {Id} apagado.");
        }
    }
}