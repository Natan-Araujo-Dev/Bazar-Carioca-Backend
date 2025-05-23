using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BazarCarioca.WebAPI.Models
{
    public class Store
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Shopkeeper? Shopkeeper { get; set; }

        public int ShopkeeperId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string? CellphoneNumber { get; set; }

        // Endereço - Antigo "Address"
        public string? Neighborhood { get; set; }

        public string? Street { get; set; }

        public int? Number { get; set; }

        public TimeOnly? OpeningTime { get; set; }

        public TimeOnly? ClosingTime { get; set; }


        [JsonIgnore]
        public ICollection<Service>? Services { get; set; }

        [JsonIgnore]
        public ICollection<ProductType>? ProductTypes { get; set; }
    }
}
