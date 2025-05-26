using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs
{
    public class ProductDTO
    {
        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool RemoveImage { get; set; } = false;

        public IFormFile? File { get; set; }

        public int? Stock { get; set; }

        public string? Description { get; set; }
    }
}
