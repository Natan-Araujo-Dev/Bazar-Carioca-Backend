namespace BazarCarioca.WebAPI.DTOs.Entities
{
    public class StorePatchRequest
    {
        public string PatchDocumentJson { get; set; }
        public IFormFile? File { get; set; }
        public bool RemoveImage { get; set; }
    }
}
