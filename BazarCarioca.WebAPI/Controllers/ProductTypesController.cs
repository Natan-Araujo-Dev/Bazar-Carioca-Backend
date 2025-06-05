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
    [Route("Bazar-Carioca/Tipos-de-Produtos")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IUnitOfWork UnityOfWork;
        private readonly IMapper Mapper;

        public ProductTypeController(IUnitOfWork _UnitOfWork, IMapper mapper)
        {
            UnityOfWork = _UnitOfWork;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productTypes = await UnityOfWork.ProductTypeRepository.GetAsync();
            if (productTypes.IsNullOrEmpty())
                return NotFound("Nenhum tipo de produto foi encontrado.");
            return Ok(productTypes);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var productType = await UnityOfWork.ProductTypeRepository.GetByIdAsync(Id);
            if (productType == null)
                return NotFound($"O tipo de produto com Id = {Id} não foi encontrado.");
            return Ok(productType);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductTypeDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var productType = Mapper.Map<ProductType>(createDto);

            await UnityOfWork.ProductTypeRepository.AddAsync(productType);
            await UnityOfWork.CommitAsync();

            var productTypeDto = Mapper.Map<ProductTypeDTO>(productType);
            return Ok(productTypeDto);
        }

        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] string requestJson)
        {
            if (requestJson == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var request = JsonConvert.DeserializeObject<JsonPatchDocument<ProductType>>(requestJson);
            var productType = await UnityOfWork.ProductTypeRepository.GetByIdAsync(Id);
            if (productType == null)
                return BadRequest($"Não existe um tipo de produto com o Id = {Id} para ser alterado.");

            var patchedProductType = await UnityOfWork.ProductTypeRepository.UpdateAsync(productType, request);
            await UnityOfWork.CommitAsync();

            var productTypeDto = Mapper.Map<ProductTypeDTO>(productType);
            return Ok(productTypeDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var productType = await UnityOfWork.ProductTypeRepository.GetByIdAsync(Id);
            if (productType == null)
                return NotFound($"O tipo de produto não foi apagado pois não existe um tipo de produto com Id = {Id}.");

            await UnityOfWork.ProductTypeRepository.DeleteAsync(Id);
            await UnityOfWork.CommitAsync();

            return Ok($"Tipo de produto com id = {Id} foi apagado.");
        }
    }
}
