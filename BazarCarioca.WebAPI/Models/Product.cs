using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        public ProductType ProductType { get; set; }

        public int ProductTypeId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public int? Stock { get; set; }

        public string? Description { get; set; }
    }
}
