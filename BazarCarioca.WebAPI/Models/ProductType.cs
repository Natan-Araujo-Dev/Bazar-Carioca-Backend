using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    public class ProductType
    {
        public int Id { get; set; }

        public Store Store { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
