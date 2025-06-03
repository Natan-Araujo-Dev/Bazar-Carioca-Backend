using AutoMapper;
using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ImageRepository<Entity, EntityUpdateDTO> : Repository<Entity>, IImageRepository<Entity, EntityUpdateDTO>
        where Entity : class, IImageEntity
        where EntityUpdateDTO : class
    {
        private readonly IWebService WebService;
        private readonly IMapper Mapper;

        public ImageRepository(AppDbContext _DataBase, IWebService _WebService, IMapper _Mapper) : base(_DataBase)
        {
            WebService = _WebService;
            Mapper = _Mapper;
        }

        public async Task<Entity> AddWithImageAsync(Entity entity, IFormFile file)
        {
            await AddAsync(entity);

            Console.WriteLine($"*** {typeof(Entity).Name}.Id é: {entity.Id}");

            var fileDirectory = $"{typeof(Entity).Name.ToLower()}s";
            var fileName = entity.Id.ToString();
            var fileUrl = await WebService.UploadImageAsync(fileDirectory, fileName, file);

            entity.ImageUrl = fileUrl;
            await CommitAsync();

            return entity;
        }

        public async Task<Entity> UpdateWithImageAsync(int Id, PatchRequestDTO requestDto)
        {
            var patchDoc = JsonConvert.DeserializeObject<JsonPatchDocument<EntityUpdateDTO>>(requestDto.PatchDocumentJson);

            var entity = await GetByIdAsync(Id);

            var updateDto = Mapper.Map<EntityUpdateDTO>(entity);

            patchDoc.ApplyTo(updateDto);

            // consertar essa lógica
            if (requestDto.File != null && !requestDto.RemoveImage)
            {
                await WebService.DeleteFileAsync(entity.ImageUrl);

                var fileDirectory = $"{typeof(Entity).Name.ToLower()}s";
                var fileName = entity.Id.ToString();
                entity.ImageUrl = await WebService.UploadImageAsync(fileDirectory, fileName, requestDto.File);
            }
            else if (requestDto.RemoveImage && entity.ImageUrl != "")
            {
                await WebService.DeleteFileAsync(entity.ImageUrl);
                entity.ImageUrl = "";
            }

            entity = Mapper.Map<Entity>(updateDto);

            await UpdateAsync(Id, entity);

            return entity;
        }
    }
}