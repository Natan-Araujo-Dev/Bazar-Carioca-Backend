using BazarCarioca.WebAPI.DTOs.Authentication;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Claim = System.Security.Claims.Claim;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("Bazar-Carioca/Autenticacao")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService TokenService;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly IConfiguration Configuration;

        public AuthController(ITokenService _TokenService, UserManager<ApplicationUser> _UserManager,
                              RoleManager<IdentityRole> _RoleManager, IConfiguration _Configuration)
        {
            TokenService = _TokenService;
            UserManager = _UserManager;
            RoleManager = _RoleManager;
            Configuration = _Configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var user = await UserManager.FindByNameAsync(model.UserName!);

            if (user is not null && await UserManager.CheckPasswordAsync(user, model.Password! ))
            {
                var userRoles = await UserManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = TokenService.GenerateAcessToken(authClaims, Configuration);

                var refreshToken = TokenService.GenerateRefreshToken();

                _ = int.TryParse(Configuration["JWT:RefreshTokenValidityInMinutes"],
                    out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await UserManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            
            return Unauthorized();
        }

        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await UserManager.FindByNameAsync(model.UserName!);

            if (userExists == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Usuário já existe." });
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "Falha na criação de usuário." });
            }

            return Ok(new Response { Status = "Sucess", Message = "Usuário criado com suceso." });
        }
    }
}
