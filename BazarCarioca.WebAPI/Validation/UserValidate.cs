using BazarCarioca.WebAPI.Models;
using BazarCarioca.WebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BazarCarioca.WebAPI.Validation
{
    public class UserValidate : IUserValidate
    {
        private readonly IUnitOfWork UnitOfWork;

        public UserValidate(IUnitOfWork _UnitOfWork)
        {
            UnitOfWork = _UnitOfWork;
        }



        public async Task<bool> IsOwner(string userEmail, IEntity entity)
        {
            if (userEmail == null
            || entity == null)
            {
                return false;
            }
            string entityName = entity.GetType().Name;
            Console.WriteLine("=========================\n\n" + entityName);

            Shopkeeper shopkeeper;

            switch (entityName)
            {
                case "Shopkeeper":
                    shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByIdAsync(entity.Id);
                break;

                case "Store":
                    shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByStoreIdAsync(entity.Id);
                break;

                case "Service":
                    shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByServiceIdAsync(entity.Id);
                break;

                case "ProductType":
                    shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByProductTypeIdAsync(entity.Id);
                break;

                case "Product":
                    shopkeeper = await UnitOfWork.ShopkeeperRepository.GetByProductIdAsync(entity.Id);
                break;

                default:
                    throw new UnauthorizedAccessException("Tipo de entidade não suportado.");
            }


            bool isTheSameEmail = await CompareEmails(userEmail, shopkeeper.Email);

            if (isTheSameEmail)
                return true;

            return false;
        }

        private async Task<bool> CompareEmails(string userEmail, string shopkeeperEmail)
        {
            if (userEmail == shopkeeperEmail)
                return true;

            return false;
        }
    }
}
