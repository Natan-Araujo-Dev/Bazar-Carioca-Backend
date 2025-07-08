using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.DTOs.Authentication
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Nome de usuário é necessário.")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email é necessário.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Uma senha é necessária.")]
        public string? Password { get; set; }
    }
}
