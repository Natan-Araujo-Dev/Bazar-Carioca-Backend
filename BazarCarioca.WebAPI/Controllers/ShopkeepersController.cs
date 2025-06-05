using AutoMapper;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("Bazar-Carioca/Lojistas")]
    [ApiController]
    public class ShopkeepersController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;

        public ShopkeepersController(IUnitOfWork _UnitOfWork, IMapper mapper)
        {
            UnitOfWork = _UnitOfWork;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var shopkeepers = await UnitOfWork.ShopkeeperRepository.GetAsync();
            if (shopkeepers.IsNullOrEmpty())
                return NotFound("Nenhum lojista foi encontrado.");
            return Ok(shopkeepers);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);
            if (shopkeeper == null)
                return NotFound($"O lojista com Id = {Id} não foi encontrado.");
            return Ok(shopkeeper);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ShopkeeperDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var shopkeeper = Mapper.Map<Shopkeeper>(createDto);
            await UnitOfWork.ShopkeeperRepository.AddAsync(shopkeeper);
            await UnitOfWork.CommitAsync();

            var shopkeeperDto = Mapper.Map<ShopkeeperDTO>(shopkeeper);
            return Ok(shopkeeperDto);
        }

        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] string requestJson)
        {
            if (requestJson == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var request = JsonConvert.DeserializeObject<JsonPatchDocument<Shopkeeper>>(requestJson);
            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);
            if (shopkeeper == null)
                return BadRequest($"Não existe um lojista com o Id = {Id} para ser alterado.");

            var patchedShopkeeper = await UnitOfWork.ShopkeeperRepository.UpdateAsync(shopkeeper, request);
            await UnitOfWork.CommitAsync();
            var shopkeeperDto = Mapper.Map<ShopkeeperDTO>(shopkeeper);
            return Ok(shopkeeperDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);
            if (shopkeeper == null)
                return NotFound($"O lojista não foi apagado pois não existe um lojista com Id = {Id}.");

            await UnitOfWork.ShopkeeperRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"Lojista com id = {Id} foi apagado.");
        }
    }
}
