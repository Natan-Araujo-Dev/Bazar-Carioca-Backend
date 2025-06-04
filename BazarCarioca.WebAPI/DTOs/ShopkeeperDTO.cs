using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.DTOs
{
    public class ShopkeeperDTO
    {
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
