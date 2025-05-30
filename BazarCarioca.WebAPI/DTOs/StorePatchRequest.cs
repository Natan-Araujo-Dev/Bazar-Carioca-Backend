namespace BazarCarioca.WebAPI.DTOs
{
    public class StorePatchRequest
    {
        public string PatchDocumentJson { get; set; }
        public IFormFile? File { get; set; }
        public bool RemoveImage { get; set; }
    }
}
