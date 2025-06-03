using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BazarCarioca.WebAPI.Models
{
    public class Product : IEntity, IImageEntity
    {
        public int Id { get; set; }

        [JsonIgnore]
        public ProductType? ProductType { get; set; }

        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int? Stock { get; set; }

        public string? Description { get; set; }
    }
}
