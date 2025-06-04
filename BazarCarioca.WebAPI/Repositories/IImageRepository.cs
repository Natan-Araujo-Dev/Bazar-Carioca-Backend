using BazarCarioca.WebAPI.DTOs;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IImageRepository<ImageEntity, EntityUpdateDTO> : IRepository<ImageEntity>
        where ImageEntity : class
    {
        Task<ImageEntity> AddWithImageAsync(ImageEntity entity, IFormFile file);
        Task<ImageEntity> UpdateWithImageAsync(int Id, PatchRequestDTO requestDTO);
    }
}
