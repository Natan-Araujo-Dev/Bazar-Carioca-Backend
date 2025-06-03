using AutoMapper;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("BazarCarioca/Lojas")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IStoreRepository Repository;
        private readonly IWebService WebService;
        private readonly IMapper Mapper;

        public StoresController(IStoreRepository repository, IWebService webService, IMapper mapper)
        {
            Repository = repository;
            WebService = webService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stores = await Repository.GetAsync();

            if (stores.IsNullOrEmpty())
                return NotFound("Nenhuma loja foi encontrada.");

            return Ok(stores);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var store = await Repository.GetByIdAsync(Id);
            
            if (store == null)
                return NotFound($"A loja com Id = {Id} não foi encontrada.");

            return Ok(store);
        }

        [HttpGet("Lojista/{Id:int}")]
        public async Task<IActionResult> GetStoresByShopkeeperId(int Id)
        {
            var stores = await Repository.GetByShopkeeperIdAsync(Id);

            if (stores == null)
                return NotFound("Lojista inexiste ou sem lojas.");

            return Ok(stores);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var store = Mapper.Map<Store>(createDto);

            if (createDto.File != null)
                store = await Repository.AddWithImageAsync(store, createDto.File);
            else
                await Repository.AddAsync(store);

            var storeDto = Mapper.Map<StoreDTO>(store);

            return Ok(storeDto);
        }

        //Só funciona com Postman
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await Repository.UpdateWithImageAsync(Id, requestDto);

            var productDto = Mapper.Map<StoreDTO>(productPatched);

            return Ok(productDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteStore(int Id)
        {
            var store = await Repository.GetByIdAsync(Id);

            if (store == null)
                return NotFound($"A loja não foi apagada pois não existe uma loja com Id = {Id}.");

            var fileUrl = store.ImageUrl;

            if (fileUrl != null)
                await WebService.DeleteFileAsync(fileUrl);

            await Repository.DeleteAsync(Id);

            return Ok($"A loja com id = {Id} foi apagada.");
        }
    }
}
