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
    [Route("bazar-carioca/servicos")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;
        private readonly IUserValidate UserValidate;

        public ServicesController(IUnitOfWork _UnitOfWork, IMapper mapper, IUserValidate _UserValidate)
        {
            UnitOfWork = _UnitOfWork;
            Mapper = mapper;
            UserValidate = _UserValidate;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var services = await UnitOfWork.ServiceRepository.GetAsync();

            if (services.IsNullOrEmpty())
                return NotFound("Nenhum serviço foi encontrado.");

            return Ok(services);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var service = await UnitOfWork.ServiceRepository.GetByIdAsync(Id);

            if (service == null)
                return NotFound($"O serviço com Id = {Id} não foi encontrado.");

            return Ok(service);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ServiceDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var service = Mapper.Map<Service>(createDto);

            await UnitOfWork.ServiceRepository.AddAsync(service);
            await UnitOfWork.CommitAsync();

            var serviceDto = Mapper.Map<Store>(service);

            return Ok(serviceDto);
        }

        //Só funciona com Postman
        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPatch]
        [Route("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] String requestJson)
        {
            var service = await UnitOfWork.ServiceRepository.GetByIdAsync(Id);

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, service);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (requestJson == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var request = JsonConvert.DeserializeObject<JsonPatchDocument<Service>>(requestJson);

            if (service == null)
                return BadRequest($"Não existe um serviço com o Id = {Id} para ser alterado.");

            var patchedService = await UnitOfWork.ServiceRepository.UpdateAsync(service, request);
            await UnitOfWork.CommitAsync();

            var serviceDto = Mapper.Map<ServiceDTO>(service);

            return Ok(serviceDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var service = await UnitOfWork.ServiceRepository.GetByIdAsync(Id);

            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, service);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (service == null)
                return NotFound($"O serviço não foi apagado pois não existe um serviço com Id = {Id}.");

            await UnitOfWork.ServiceRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"A loja com id = {Id} foi apagada.");
        }
    }
}