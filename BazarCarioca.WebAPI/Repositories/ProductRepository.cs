using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IWebService WebService;
        private readonly IMapper Mapper;

        public ProductRepository(AppDbContext _DataBase, IWebService _WebService, IMapper _Mapper) : base(_DataBase)
        {
            WebService = _WebService;
            Mapper = _Mapper;
        }

        public async Task<Product> AddWithImageAsync(Product product, IFormFile file)
        {
            await AddAsync(product);

            Console.WriteLine("Id é: " + product.Id);

            var fileName = product.Id.ToString();
            var fileUrl = await WebService.UploadImageAsync("products", fileName, file);

            product.ImageUrl = fileUrl;
            await CommitAsync();

            return product;
        }

        public async Task<Product> UpdateWithImageAsync (int Id, ProductPatchRequestDTO requestDto)
        {
            var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<ProductUpdateDTO>>(requestDto.PatchDocumentJson);

            var product = await GetByIdAsync(Id);

            var updateDto = Mapper.Map<ProductUpdateDTO>(product);

            patchDoc.ApplyTo(updateDto);

            // consertar essa lógica
            if (requestDto.File != null && !requestDto.RemoveImage)
            {
                await WebService.DeleteFileAsync(product.ImageUrl);

                var fileName = product.Id.ToString();
                product.ImageUrl = await WebService.UploadImageAsync("products", fileName, requestDto.File);
            }
            else if (requestDto.RemoveImage && product.ImageUrl != "")
            {
                await WebService.DeleteFileAsync(product.ImageUrl);
                product.ImageUrl = "";
            }

            // colocar mapper
            product.ProductTypeId = updateDto.ProductTypeId;
            product.Name = updateDto.Name;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.Description = updateDto.Description;

            await UpdateAsync(Id, product);

            return product;
        }
    }
}
