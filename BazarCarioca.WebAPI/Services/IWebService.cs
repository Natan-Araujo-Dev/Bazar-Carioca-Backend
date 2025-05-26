namespace BazarCarioca.WebAPI.Services
{
    public interface IWebService
    {
        Task<string> UploadFileAsync(IFormFile file);
    }
}
