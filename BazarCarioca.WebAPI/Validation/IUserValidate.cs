using BazarCarioca.WebAPI.Models;

namespace BazarCarioca.WebAPI.Validation
{
    public interface IUserValidate
    {
        Task<bool> IsOwner(string userEmail, IEntity entity);
    }
}
