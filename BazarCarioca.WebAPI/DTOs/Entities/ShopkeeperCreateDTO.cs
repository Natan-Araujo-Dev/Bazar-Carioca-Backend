using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.DTOs.Entities
{
    public class ShopkeeperCreateDTO
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
