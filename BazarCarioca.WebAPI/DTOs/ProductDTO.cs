using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs
{
    public class ProductDTO
    {
        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int? Stock { get; set; }

        public string? Description { get; set; }
    }
}
