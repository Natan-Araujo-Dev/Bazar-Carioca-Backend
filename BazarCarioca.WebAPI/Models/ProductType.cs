using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BazarCarioca.WebAPI.Models
{
    public class ProductType : IEntity
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Store? Store { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
