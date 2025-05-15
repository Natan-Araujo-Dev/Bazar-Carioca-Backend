using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopkeepersController : ControllerBase
    {

        // Código feito para testar o mapping
        //[HttpDelete("DeletaLojista/{id:int}")]
        //public async Task<IActionResult> DeleteShopkeeper(int id)
        //{
        //    var shopkeeper = await DataBase.Shopkeepers
        //        .FindAsync(id);

        //    if (shopkeeper == null)
        //    {
        //        return NotFound("Nenhum lojista bro");
        //    }

        //    DataBase.Shopkeepers.Remove(shopkeeper);
        //    await DataBase.SaveChangesAsync();

        //    return Ok("Deletou mermo hein. Agora confere lá no BDD");
        //}
    }
}
