namespace BazarCarioca.WebAPI.DTOs.Entities
{
    public class ProductCreateDTO
    {
        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public IFormFile? File { get; set; }

        public int? Stock { get; set; }

        public string? Description { get; set; }
    }
}
