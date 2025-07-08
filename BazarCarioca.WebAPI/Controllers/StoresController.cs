﻿using AutoMapper;
using BazarCarioca.WebAPI.DTOs.Entities;
using BazarCarioca.WebAPI.Extensions;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("Bazar-Carioca/Lojas")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IWebService WebService;
        private readonly IMapper Mapper;

        public StoresController(IUnitOfWork _UnitOfWork, IWebService webService, IMapper mapper)
        {
            UnitOfWork = _UnitOfWork;
            WebService = webService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stores = await UnitOfWork.StoreRepository.GetAsync();

            if (stores.IsNullOrEmpty())
                return NotFound("Nenhuma loja foi encontrada.");

            return Ok(stores);
        }

        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var store = await UnitOfWork.StoreRepository.GetByIdAsync(Id);
            
            if (store == null)
                return NotFound($"A loja com Id = {Id} não foi encontrada.");

            return Ok(store);
        }

        [HttpGet("Lojista/{Id:int}")]
        public async Task<IActionResult> GetByShopkeeperId(int Id)
        {
            var stores = await UnitOfWork.StoreRepository.GetByShopkeeperIdAsync(Id);

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
        [HttpPatch("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Patch(int Id, [FromForm] PatchRequestDTO requestDto)
        {
            if (requestDto == null)
                return BadRequest("Houve um erro na requisição HTTP. Informações não foram enviadas.");

            var productPatched = await UnitOfWork.StoreRepository.UpdateWithImageAsync(Id, requestDto);
            await UnitOfWork.CommitAsync();

            var productDto = Mapper.Map<StoreDTO>(productPatched);

            return Ok(productDto);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteStore(int Id)
        {
            var store = await UnitOfWork.StoreRepository.GetByIdAsync(Id);

            if (store == null)
                return NotFound($"A loja não foi apagada pois não existe uma loja com Id = {Id}.");

            var fileUrl = store.ImageUrl;

            if (fileUrl != null)
                await WebService.DeleteFileAsync(fileUrl);

            await UnitOfWork.StoreRepository.DeleteAsync(Id);
            await UnitOfWork.CommitAsync();

            return Ok($"A loja com id = {Id} foi apagada.");
        }

        /// <summary>
        /// Método somente para desenvolvimento. NÂO implemente.
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var stores = await UnitOfWork.StoreRepository.GetAsync();

            if (stores.IsNullOrEmpty())
                return NotFound("Nenhuma loja foi apagada pois não existem lojas cadastradas.");

            var ids = new List<int>();
            var imageUrls = new List<string>();

            foreach (var store in stores)
            {
                ids.Add(store.Id);
                imageUrls.Add(store.ImageUrl);
            }

            for (int i = 0; i < ids.Count; i++)
            {
                if (imageUrls[i] != null)
                    await WebService.DeleteFileAsync(imageUrls[i]);

                await UnitOfWork.StoreRepository.DeleteAsync(ids[i]);
                await UnitOfWork.CommitAsync();
            }

            return Ok("Todos lojas e suas imagens foram apagadas.");
        }
    }
}
