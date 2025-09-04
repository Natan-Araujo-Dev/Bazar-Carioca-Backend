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
    [Route("bazar-carioca/produtos")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IWebService WebService;
        private readonly IMapper Mapper;
        private readonly IUserValidate UserValidate;

        public ProductsController(IUnitOfWork _UnitOfWork, IWebService webService, IMapper mapper,
            IUserValidate _UserValidate)
        {
            UnitOfWork = _UnitOfWork;
            WebService = webService;
            Mapper = mapper;
            UserValidate = _UserValidate;
        }



        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await UnitOfWork.ProductRepository.GetAsync();

            if (products.IsNullOrEmpty())
                return NotFound("Nenhum produto foi encotrado.");

            var productsDTO = Mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet]
        [Route("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(Id);

            if (product == null)
                return NotFound($"O produto com Id = {Id} não foi encontrado.");

            var productDTO = Mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpGet]
        [Route("tipo-de-produto/{Id:int}")]
        public async Task<IActionResult> GetByProductTypeId(int Id)
        {
            var produtos = await UnitOfWork.ProductRepository.GetByProductTypeIdAsync(Id);

            if (produtos.IsNullOrEmpty())
                return NotFound("Tipo de produto inexiste ou sem produtos.");

            return Ok(produtos);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateDTO createDto)
        {
            if (createDto == null)
                return BadRequest("Nenhuma informação foi passada na requisição.");

            var product = Mapper.Map<Product>(createDto);

            var productType = UnitOfWork.ProductTypeRepository.GetByIdAsync(product.ProductTypeId).Result;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, productType);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (createDto.File != null)
            {
                product = await UnitOfWork.ProductRepository.AddWithImageAsync(product, createDto.File);
                await UnitOfWork.CommitAsync();
            }
            else
            {
                await UnitOfWork.ProductRepository.AddAsync(product);
                await UnitOfWork.CommitAsync();
            }

            var productDto = Mapper.Map<ProductDTO>(product);

            return Ok(productDto);
        }

        //Só funciona com Postman por causa do JSON de PATCH
        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpPatch]
        [Route("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(Id);

            var productType = UnitOfWork.ProductTypeRepository.GetByIdAsync(product.ProductTypeId).Result;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, productType);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await UnitOfWork.ProductRepository.UpdateWithImageAsync(Id, requestDto);
            await UnitOfWork.CommitAsync();

            var productDto = Mapper.Map<ProductDTO>(productPatched);

            return Ok(productDto);
        }

        [Authorize(Roles = "SuperAdmin,Admin,Shopkeeper")]
        [HttpDelete]
        [Route("{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var product = await UnitOfWork.ProductRepository.GetByIdAsync(Id);

            var productType = UnitOfWork.ProductTypeRepository.GetByIdAsync(product.ProductTypeId).Result;
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var isOwner = await UserValidate.IsOwner(userEmail, productType);

            if (User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "SuperAdmin"
            && User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value != "Admin"
            && !isOwner)
            {
                return Unauthorized();
            }

            if (product == null)
                return NotFound($"O produto não foi apagado pois não existe um produto com Id = {Id}.");

            var fileUrl = product.ImageUrl;

            if (fileUrl != null)
                await WebService.DeleteFileAsync(fileUrl);

            await UnitOfWork.ProductRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"O produto com id = {Id} apagado.");
        }
    }
}