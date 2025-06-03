using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BazarCarioca.WebAPI.Models
{
    public class Shopkeeper : IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }


        [JsonIgnore]
        public ICollection<Store>? Stores { get; set; }
    }
}
