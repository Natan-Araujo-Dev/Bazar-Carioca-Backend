namespace BazarCarioca.WebAPI.Services
{
    public interface IWebService
    {
        Task<string> UploadImageAsync(string entityDirectory, IFormFile file);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
