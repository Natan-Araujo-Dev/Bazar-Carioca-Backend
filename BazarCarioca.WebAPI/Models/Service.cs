using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BazarCarioca.WebAPI.Models
{
    public class Service
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Store? Store { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public decimal? AveragePrice { get; set; }
    }
}
