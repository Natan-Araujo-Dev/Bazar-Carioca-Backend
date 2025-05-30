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

        public StoresController(IStoreRepository repository, IWebService webService)
        {
            Repository = repository;
            WebService = webService;
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
        public async Task<ActionResult<StoreDTO>> Create([FromForm] StoreCreateDTO dto)
        {
            var fileUrl = "";

            if (dto.File != null)
                fileUrl = await WebService.UploadImageAsync("stores", dto.File);

            // substituir por mapper
            var store = new Store
            {
                ShopkeeperId = dto.ShopkeeperId,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = fileUrl,
                CellphoneNumber = dto.CellphoneNumber,
                Neighborhood = dto.Neighborhood,
                Street = dto.Street,
                Number = dto.Number,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime
            };

            await Repository.AddAsync(store);

            // substituir por mapper
            var finalDto = new StoreDTO
            {
                ShopkeeperId = dto.ShopkeeperId,
                Name = dto.Name,
                Description = dto.Description,
                ImageUrl = fileUrl,
                CellphoneNumber = dto.CellphoneNumber,
                Neighborhood = dto.Neighborhood,
                Street = dto.Street,
                Number = dto.Number,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime
            };

            return Ok(finalDto);
        }

        //Só funciona com Postman
        //Refinar lógica principalmente com WebService
        [HttpPatch("{id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<StoreDTO>> Patch(int id, [FromForm] StorePatchRequest requestDto)
        {
            var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<StoreUpdateDTO>>(requestDto.PatchDocumentJson);

            var store = await Repository.GetByIdAsync(id);

            var dto = new StoreUpdateDTO
            {
                ShopkeeperId = store.ShopkeeperId,
                Name = store.Name,
                Description = store.Description,
                CellphoneNumber = store.CellphoneNumber,
                Neighborhood = store.Neighborhood,
                Street = store.Street,
                Number = store.Number,
                OpeningTime = store.OpeningTime,
                ClosingTime = store.ClosingTime
            };

            patchDoc.ApplyTo(dto, ModelState);

            if (requestDto.File != null && !requestDto.RemoveImage)
            {
                await WebService.DeleteFileAsync(store.ImageUrl);
                store.ImageUrl = await WebService.UploadImageAsync("stores", requestDto.File);
            }
            else if (requestDto.RemoveImage && store.ImageUrl != "")
            {
                await WebService.DeleteFileAsync(store.ImageUrl);
                store.ImageUrl = "";
            }

            store.ShopkeeperId = dto.ShopkeeperId;
            store.Name = dto.Name;
            store.Description = dto.Description;
            store.CellphoneNumber = dto.CellphoneNumber;
            store.Neighborhood = dto.Neighborhood;
            store.Street = dto.Street;
            store.Number = dto.Number;
            store.OpeningTime = dto.OpeningTime;
            store.ClosingTime = dto.ClosingTime;

            await Repository.UpdateAsync(id, store);

            var result = new StoreDTO
            {
                ShopkeeperId = store.ShopkeeperId,
                Name = store.Name,
                Description = store.Description,
                ImageUrl = store.ImageUrl,
                CellphoneNumber = store.CellphoneNumber,
                Neighborhood = store.Neighborhood,
                Street = store.Street,
                Number = store.Number,
                OpeningTime = store.OpeningTime,
                ClosingTime = store.ClosingTime
            };

            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<ActionResult<bool>> DeleteStore(int Id)
        {
            await Repository.DeleteAsync(Id);

            return Ok($"Loja com id = {Id} apagada.");
        }
    }
}
