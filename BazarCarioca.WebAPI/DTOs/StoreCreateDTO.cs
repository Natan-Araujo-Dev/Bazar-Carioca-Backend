using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs
{
    public class StoreCreateDTO
    {
        public int ShopkeeperId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public IFormFile? File { get; set; }

        public string? CellphoneNumber { get; set; }

        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public int? Number { get; set; }

        public TimeOnly? OpeningTime { get; set; }

        public TimeOnly? ClosingTime { get; set; }
    }
}
