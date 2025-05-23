namespace BazarCarioca.WebAPI.Services
{
    public interface IS3Service
    {
        Task UploadFileAsync(IFormFile file);
    }
}
