using BazarCarioca.WebAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using static Amazon.S3.Util.S3EventNotification;

namespace BazarCarioca.WebAPI.Repositories
{
    public interface IShopkeeperRepository : IRepository<Shopkeeper>
    {
        Task<Shopkeeper> GetByStoreIdAsync(int Id);
        Task<Shopkeeper> GetByServiceIdAsync(int Id);
        Task<Shopkeeper> GetByProductTypeIdAsync(int Id);
        Task<Shopkeeper> GetByProductIdAsync(int Id);
        Task<Shopkeeper> GetByEmail(string email);
        Task<bool> EmailInUse(string email);
    }
}
