using System.ComponentModel.DataAnnotations;

namespace BazarCarioca.WebAPI.DTOs.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Nome de usuário é necessário.")]
        public string? UserEmail { get; set; }

        [Required(ErrorMessage = "Uma senha é necessária.")]
        public string? Password { get; set; }
    }
}
