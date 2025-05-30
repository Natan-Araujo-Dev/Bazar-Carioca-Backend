namespace BazarCarioca.WebAPI.Services
{
    public interface IWebService
    {
        Task<string> UploadImageAsync(string entityDirectory, string fileName, IFormFile file);
        Task<bool> DeleteFileAsync(string fileUrl);
    }
}
