using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    public class Store
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public int? CellphoneNumber { get; set; }

        // Endereço - Antigo "Address"
        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public int? Number { get; set; }

        public TimeOnly? OpeningTime { get; set; }

        public TimeOnly? ClosingTime { get; set; }



        public ICollection<Service>? Services { get; set; }

        public ICollection<ProductType>? ProductTypes { get; set; }

        public Shopkeeper Shopkeeper { get; set; }
    }
}
