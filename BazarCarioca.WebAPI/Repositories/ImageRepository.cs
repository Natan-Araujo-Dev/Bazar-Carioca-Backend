using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.DTOs;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ImageRepository<Entity> : Repository<Entity>, IImageRepository<Entity>
        where Entity : class, IImageEntity
    {
        private readonly IWebService WebService;

        public ImageRepository(AppDbContext _DataBase, IWebService _WebService) : base(_DataBase)
        {
            WebService = _WebService;
        }

        public async Task<Entity> AddWithImageAsync(Entity entity, IFormFile file)
        {
            await AddAsync(entity);

            Console.WriteLine($"*** {typeof(Entity).Name}.Id é: {entity.Id}");

            var fileDirectory = $"{typeof(Entity).Name}s";
            var fileName = entity.Id.ToString();
            var fileUrl = await WebService.UploadImageAsync(fileDirectory, fileName, file);

            entity.ImageUrl = fileUrl;
            await CommitAsync();

            return entity;
        }

        public Task<Entity> UpdateWithImageAsync(int Id, PatchRequestDTO requestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
