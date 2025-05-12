using BazarCarioca.WebAPI.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopkeepersController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ShopkeepersController(AppDbContext context)
        {
            _context = context;
        }

        // Código feito para testar o mapping
        [HttpDelete("DeletaLojista/{id:int}")]
        public async Task<IActionResult> DeleteShopkeeper(int id)
        {
            var shopkeeper = await _context.Shopkeepers
                .FindAsync(id);

            if (shopkeeper == null)
            {
                return NotFound("Nenhum lojista bro");
            }

            _context.Shopkeepers.Remove(shopkeeper);
            await _context.SaveChangesAsync();

            return Ok("Deletou mermo hein. Agora confere lá no BDD");
        }
    }
}
