using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("bazar-carioca/busca")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;

        public SearchController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string tiposDeEntidades, string termo)
        {
            if (string.IsNullOrEmpty(termo))
                return BadRequest("Termo de busca não informado.");

            termo = termo.ToLower();

            if (tiposDeEntidades == "todos")
            {
                var lojas = await UnitOfWork.StoreRepository.GetByTermAsync(termo);

                var servicos = await UnitOfWork.ServiceRepository.GetByTermAsync(termo);

                var tiposDeProdutos = await UnitOfWork.ProductTypeRepository.GetByTermAsync(termo);

                var produtos = await UnitOfWork.ProductRepository.GetByTermAsync(termo);

                var searchResult = new SearchDTO
                {
                    Stores = Mapper.Map<IEnumerable<StoreDTO>>(lojas),
                    Services = Mapper.Map<IEnumerable<ServiceDTO>>(servicos),
                    ProductTypes = Mapper.Map<IEnumerable<ProductTypeDTO>>(tiposDeProdutos),
                    Products = Mapper.Map<IEnumerable<ProductDTO>>(produtos)
                };

                if (!searchResult.Stores.Any() && !searchResult.Services.Any()
                && !searchResult.ProductTypes.Any() && !searchResult.Products.Any())
                {
                    return NotFound("Nenhum resultado encontrado para o termo de busca informado.");
                }

                return Ok(searchResult);
            }

            switch (tiposDeEntidades)
            {
                case "lojas":
                    var lojas = await UnitOfWork.StoreRepository.GetByTermAsync(termo);
                    if (!lojas.Any())
                        return NotFound("Nenhum resultado encontrado para o termo de busca informado.");
                    return Ok(Mapper.Map<IEnumerable<StoreDTO>>(lojas));
                case "servicos":
                    var servicos = await UnitOfWork.ServiceRepository.GetByTermAsync(termo);
                    if (!servicos.Any())
                        return NotFound("Nenhum resultado encontrado para o termo de busca informado.");
                    return Ok(Mapper.Map<IEnumerable<ServiceDTO>>(servicos));
                case "tipos-de-produtos":
                    var tiposDeProdutos = await UnitOfWork.ProductTypeRepository.GetByTermAsync(termo);
                    if (!tiposDeProdutos.Any())
                        return NotFound("Nenhum resultado encontrado para o termo de busca informado.");
                    return Ok(Mapper.Map<IEnumerable<ProductTypeDTO>>(tiposDeProdutos));
                case "produtos":
                    var produtos = await UnitOfWork.ProductRepository.GetByTermAsync(termo);
                    if (!produtos.Any())
                        return NotFound("Nenhum resultado encontrado para o termo de busca informado.");
                    return Ok(Mapper.Map<IEnumerable<ProductDTO>>(produtos));
                default:
                    return BadRequest("Entidade inválida. As entidades válidas são: todas, lojas, servicos, tipos-de-produtos, produtos.");
            }
        }
    }
}