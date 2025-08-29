using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using BazarCarioca.WebAPI.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("bazar-carioca/lojas")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IWebService WebService;
        private readonly IMapper Mapper;
        private readonly IUserValidate UserValidate;

        public StoresController(IUnitOfWork _UnitOfWork, IWebService _WebService, IMapper _Mapper, 
            IUserValidate _UserValidate)
        {
            UnitOfWork = _UnitOfWork;
            WebService = _WebService;
            Mapper = _Mapper;
            UserValidate = _UserValidate;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stores = await UnitOfWork.StoreRepository.GetAsync();

            if (stores.IsNullOrEmpty())
                return NotFound("Nenhuma loja foi encontrada.");

            return Ok(stores);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var store = await UnitOfWork.StoreRepository.GetByIdAsync(Id);
            
            if (store == null)
                return NotFound($"A loja com Id = {Id} não foi encontrada.");

            return Ok(store);
        }

        [HttpGet]
        [Route("lojista/{Id:int}")]
        public async Task<IActionResult> GetByShopkeeperId(int Id)
        {
            var stores = await UnitOfWork.StoreRepository.GetByShopkeeperIdAsync(Id);

            if (stores.IsNullOrEmpty())
                return NotFound("Lojista inexiste ou sem lojas.");

            return Ok(stores);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] StoreCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var store = Mapper.Map<Store>(createDto);

            if (createDto.File != null)
            {
                store = await UnitOfWork.StoreRepository.AddWithImageAsync(store, createDto.File);
                await UnitOfWork.CommitAsync();
            }
            else
            {
                await UnitOfWork.StoreRepository.AddAsync(store);
                await UnitOfWork.CommitAsync();
            }

            var storeDto = Mapper.Map<StoreDTO>(store);

            return Ok(storeDto);
        }

        //Só funciona com Postman
        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPatch]
        [Route("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var store = await UnitOfWork.StoreRepository.GetByIdAsync(Id);
            var isOwner = await UserValidate.IsOwner(userEmail, store);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized("Você não tem autorização para alterar esta loja.");
            }

            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await UnitOfWork.StoreRepository.UpdateWithImageAsync(Id, requestDto);
            await UnitOfWork.CommitAsync();

            var productDto = Mapper.Map<StoreDTO>(productPatched);

            return Ok(productDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> DeleteStore(int Id)
        {
            var store = await UnitOfWork.StoreRepository.GetByIdAsync(Id);

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, store);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (store == null)
                return NotFound($"A loja não foi apagada pois não existe uma loja com Id = {Id}.");

            var fileUrl = store.ImageUrl;

            if (fileUrl != null)
                await WebService.DeleteFileAsync(fileUrl);

            await UnitOfWork.StoreRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"A loja com id = {Id} foi apagada.");
        }
    }
}
