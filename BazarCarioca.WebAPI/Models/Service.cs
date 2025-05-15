using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    public class Service
    {
        public int Id { get; set; }

        public Store Store { get; set; }

        public int StoreId { get; set; }

        public string Name { get; set; }

        public decimal? AveragePrice { get; set; }
    }
}
