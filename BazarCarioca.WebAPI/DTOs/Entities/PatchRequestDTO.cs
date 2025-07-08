namespace BazarCarioca.WebAPI.DTOs.Entities
{
    public class PatchRequestDTO
    {
        public string PatchDocumentJson { get; set; }
        public IFormFile? File { get; set; }
        public bool RemoveImage { get; set; }
    }
}
