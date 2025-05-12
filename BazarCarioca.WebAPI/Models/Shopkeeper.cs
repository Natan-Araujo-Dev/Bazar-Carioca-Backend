using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.Models
{
    public class Shopkeeper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }



        public ICollection<Store>? Stores { get; set; }
    }
}
