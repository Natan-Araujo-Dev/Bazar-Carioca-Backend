using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.DTOs
{
    public class ServiceDTO
    {
        public int StoreId { get; set; }

        public string Name { get; set; }

        public decimal? AveragePrice { get; set; }
    }
}
