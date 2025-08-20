using BazarCarioca.WebAPI.DTOs.Authentication;
using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Claim = System.Security.Claims.Claim;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("bazar-carioca/autenticacao")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService TokenService;
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly IConfiguration Configuration;
        private readonly ILogger<AuthController> Logger;

        public AuthController(ITokenService _TokenService, UserManager<ApplicationUser> _UserManager,
                              RoleManager<IdentityRole> _RoleManager, IConfiguration _Configuration,
                              ILogger<AuthController> _logger)
        {
            TokenService = _TokenService;
            UserManager = _UserManager;
            RoleManager = _RoleManager;
            Configuration = _Configuration;
            Logger = _logger;
        }

        [HttpPost]
        [Route("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await RoleManager.RoleExistsAsync(roleName);

            if (roleExists)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    { 
                        Status = "Erro",
                        Message = "Função já existe."
                    }
                );
            }

            var roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));

            if (!roleResult.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response 
                    { 
                        Status = "Erro",
                        Message = "Falha ao criar função."
                    }
                );
            }

            Logger.LogInformation(1, $"Role {roleName} added");

            return StatusCode(
                statusCode: StatusCodes.Status200OK,
                new Response 
                { 
                    Status = "Sucesso",
                    Message = $"Função {roleName} criada com sucesso."
                }
            );
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        [Route("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var result = await UserManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                Logger.LogInformation(1, $"Falha ao adicionar usuário de email {email} à função {roleName}.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response 
                    {
                        Status = "Erro",
                        Message = $"Falha ao adicionar usuário de email {email} à função {roleName}." 
                    }
                );
            }

            Logger.LogInformation(1, $"Usuário de email {email} adicionado à função {roleName}.");

            return StatusCode(
                StatusCodes.Status200OK,
                new Response
                {
                    Status = "Sucesso",
                    Message = $"Usuário de email {email} adicionado à função {roleName}."
                }
            );
        }

        [HttpPost]
        [Route("add-user-to-shopkeeper")]
        public async Task<IActionResult> AddUserToShopkeeper(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Usuário não encontrado.");
            }

            var result = await UserManager.AddToRoleAsync(user, "Shopkeeper");

            if (!result.Succeeded)
            {
                Logger.LogInformation(1, $"Falha ao adicionar usuário de email {email} à função Shopkeeper.");

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Erro",
                        Message = $"Falha ao adicionar usuário de email {email} à função Shopkeeper."
                    }
                );
            }

            Logger.LogInformation(1, $"Usuário de email {email} adicionado à função Shopkeeper.");

            return StatusCode(
                StatusCodes.Status200OK,
                new Response
                {
                    Status = "Sucesso",
                    Message = $"Usuário de email {email} adicionado à função Shopkeeper."
                }
            );
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            var user = await UserManager.FindByEmailAsync(model.UserEmail!);

            if (user == null
            || !await UserManager.CheckPasswordAsync(user, model.Password!))
            {
                return Unauthorized();
            }

            var userRoles = await UserManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                //new Claim("id", user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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

        [HttpPost]
        [Route("registrar")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var emailExists = await UserManager.FindByEmailAsync(model.Email!);

            if (emailExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response { 
                        Status = "Erro", 
                        Message = $"O email {model.Email} já está em uso." 
                    }
                );
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await UserManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new Response 
                    { 
                        Status = "Erro", 
                        Message = "Falha na criação de usuário." 
                    }
                );
            }

            return Ok(
                new Response 
                { 
                    Status = "Sucesso", 
                    Message = "Usuário criado com suceso."
                }
            );
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Requisição do cliente inválida.");
            }
            string? acessToken = tokenModel.AcessToken
                                ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                              ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = TokenService.GetPrincipalFromExpiredToken(acessToken!, Configuration);

            if (principal == null)
            {
                return BadRequest("Token ou refresh token inválido.");
            }

            string username = principal.Identity.Name;
            var user = await UserManager.FindByNameAsync(username!);

            if (user == null
            || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Token ou refresh token inválido.");
            }

            var newAcessToken = TokenService.GenerateAcessToken(
                principal.Claims.ToList(), Configuration);

            var newRefreshToken = TokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await UserManager.UpdateAsync(user); 

            return new ObjectResult(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAcessToken),
                RefreshToken = newRefreshToken,
            });
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await UserManager.FindByNameAsync(username);

            if (user == null)
            {
                return BadRequest("Nome de usuário inválido.");
            }

            user.RefreshToken = null;

            await UserManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
