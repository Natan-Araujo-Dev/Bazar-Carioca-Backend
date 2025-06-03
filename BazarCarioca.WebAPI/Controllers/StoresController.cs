using AutoMapper;
using BazarCarioca.WebAPI.DTOs;
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

        [HttpPost]
        public async Task<ActionResult<StoreDTO>> Create([FromForm] StoreCreateDTO createDto)
        {
            var store = Mapper.Map<Store>(createDto);

            if (createDto.File != null)
                store = await Repository.AddWithImageAsync(store, createDto.File);
            else
                await Repository.AddAsync(store);

            var storeDto = Mapper.Map<StoreDTO>(store);

            return Ok(storeDto);
        }

        //Só funciona com Postman
        //Refinar lógica principalmente com WebService
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<StoreDTO>> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            var productPatched = await Repository.UpdateWithImageAsync(Id, requestDto);

            if (requestDto.File != null)
                Console.WriteLine("***** Pelo visto NÂO é null");
            else
                Console.WriteLine("***** Pelo visto é null");

            var productDto = Mapper.Map<StoreDTO>(productPatched);

            return Ok(productDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Loja com id = {Id} apagada.");
        }
    }
}
