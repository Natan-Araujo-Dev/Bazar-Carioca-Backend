using BazarCarioca.WebAPI.Context;
using BazarCarioca.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Repositories
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        public ProductTypeRepository(AppDbContext _DataBase) : base(_DataBase)
        {
        }

        public async Task<IEnumerable<ProductType>> GetByStoreIdAsync(int Id)
        {
            var productTypes = await DataBase.ProductTypes
                 .Where(s => s.StoreId == Id)
                 .ToListAsync();

            if (productTypes is null)
                throw new ArgumentException(nameof(ProductType));

            return productTypes;
        }

        public async Task<IEnumerable<ProductType>> GetTreatedByStoreIdAsync(int Id)
        {
            var productTypes = await DataBase.ProductTypes
                 .Where(s => s.StoreId == Id)
                 .ToListAsync();

            foreach (var productType in productTypes)
            {
                productType.Products = await DataBase.Products
                    .Where(p => p.ProductTypeId == productType.Id)
                    .ToListAsync();
            }

            if (productTypes is null)
                throw new ArgumentException(nameof(ProductType));

            return productTypes;
        }

        public async Task<IEnumerable<ProductType>> GetByTermAsync(string Term)
        {
            var productTypes = await DataBase.ProductTypes
                    .Where(x => x.Name.ToLower().Contains(Term))
                    .ToListAsync();

            return productTypes;
        }
    }
}
