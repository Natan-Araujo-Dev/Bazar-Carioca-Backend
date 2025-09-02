using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("bazar-carioca/lojistas")]
    [ApiController]
    //[Authorize]
    public class ShopkeepersController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;
        private readonly IUserValidate UserValidate;

        public ShopkeepersController(IUnitOfWork _UnitOfWork, IMapper mapper, IUserValidate _UserValidate)
        {
            UnitOfWork = _UnitOfWork;
            Mapper = mapper;
            UserValidate = _UserValidate;
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var shopkeepers = await UnitOfWork.ShopkeeperRepository.GetAsync();
            if (shopkeepers.IsNullOrEmpty())
                return NotFound("Nenhum lojista foi encontrado.");

            return Ok(shopkeepers);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);
            if (shopkeeper == null)
                return NotFound($"O lojista com Id = {Id} não foi encontrado.");
            return Ok(shopkeeper);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [Route("email")]
        public async Task<IActionResult> GetByEmail([FromQuery] string Email)
        {

            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByEmail(Email);

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, shopkeeper);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized("Esse não é o seu email.");
            }

            if (shopkeeper == null)
                return NotFound($"O lojista com email = {Email} não foi encontrado.");


            var shopkeeperDto = Mapper.Map<ShopkeeperDTO>(shopkeeper);
            return Ok(shopkeeperDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ShopkeeperCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var emailExists = await UnitOfWork.ShopkeeperRepository.EmailInUse(createDto.Email!);
            if (emailExists)
                return BadRequest($"O email '{createDto.Email}' já está em uso.");

            var shopkeeper = Mapper.Map<Shopkeeper>(createDto);
            await UnitOfWork.ShopkeeperRepository.AddAsync(shopkeeper);
            await UnitOfWork.CommitAsync();

            var shopkeeperDto = Mapper.Map<ShopkeeperDTO>(shopkeeper);
            return Ok(shopkeeperDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPatch]
        [Route("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] string requestJson)
        {
            if (requestJson == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);

            if (shopkeeper == null)
                return BadRequest($"Não existe um lojista com o Id = {Id} para ser alterado.");

            var request = JsonConvert.DeserializeObject<JsonPatchDocument<Shopkeeper>>(requestJson);

            var patchedShopkeeper = await UnitOfWork.ShopkeeperRepository.UpdateAsync(shopkeeper, request);
            await UnitOfWork.CommitAsync();
            var shopkeeperDto = Mapper.Map<ShopkeeperDTO>(shopkeeper);
            return Ok(shopkeeperDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(Id);
            var isOwner = await UserValidate.IsOwner(userEmail, shopkeeper);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized("Você não tem autorização para apagar esse lojista.");
            }

            if (shopkeeper == null)
                return NotFound($"O lojista não foi apagado pois não existe um lojista com Id = {Id}.");

            await UnitOfWork.ShopkeeperRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"Lojista com id = {Id} foi apagado.");
        }
    }
}
